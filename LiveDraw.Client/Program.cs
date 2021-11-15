using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;

namespace LiveDraw.Client
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            //System.Diagnostics.Debugger.Launch();
            //System.Diagnostics.Debugger.Break();

            var assembly = Assembly.GetExecutingAssembly();
            var dir = Path.GetDirectoryName(assembly.Location);

            try
            {
                if (!Process.GetProcessesByName("LiveDraw").Any())
                {
                    var liveDraw = Path.Combine(dir, "LiveDraw.exe");
                    var info = new ProcessStartInfo(liveDraw);
                    info.WorkingDirectory = dir;
                    info.UseShellExecute = false;
                    Process.Start(info);

                    // TODO: wait until started
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to start LiveDraw.\r\n{ex.Message}");
            }
                        
            X509Certificate2 serverCertificate = new X509Certificate2(Path.Combine(dir, "livedraw.pfx"), "livedraw");                       

            var handler = new HttpClientHandler { 
                UseDefaultCredentials = true,
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) =>
                {
                    return cert?.Thumbprint == serverCertificate.Thumbprint;
                }
            };
            var client = new HttpClient(handler);

           
            client.BaseAddress = new Uri("https://localhost:5001/");

            var name = Process.GetCurrentProcess().ProcessName;
            if (name is not null)
            {
                var post = name.Replace('_', '/');
                var endpoint = "LiveDraw/" + post;
                var op = $"POST to {client.BaseAddress}{endpoint}";

                // MessageBox.Show(op);

                var identity = WindowsIdentity.GetCurrent();
                await WindowsIdentity.RunImpersonated(identity.AccessToken, async () =>
                {
                    try
                    {
                        var response = await client.PostAsync(endpoint, null);
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            var msg = $"Failed to {op}. Response: {response.StatusCode}.";
                            MessageBox.Show(msg);
                        }
                    }
                    catch (HttpRequestException hre)
                    {
                        MessageBox.Show($"{op}\r\n{hre.Message}");
                    }
                });
            }
        }
    }
}