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
            HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
            client.BaseAddress = new Uri("https://localhost:5001/");

            var name = Process.GetCurrentProcess().ProcessName;
            if (name is not null)
            {
                var post = name.Replace('_', '/');
                var endpoint = "LiveDraw/" + post;
                var op = $"POST to {client.BaseAddress}{endpoint}";

                MessageBox.Show(op);

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