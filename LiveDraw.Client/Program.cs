using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace LiveDraw.Client
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            try
            {
                if (!Process.GetProcessesByName("LiveDraw").Any())
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var dir = Path.GetDirectoryName(assembly.Location);
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

            HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
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