using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace LiveDraw.PreviousColor
{
    internal static class Program
    {
        [STAThread]
        static async Task Main()
        {
            HttpClient client = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });
            client.BaseAddress = new Uri("https://localhost:5001/");

            var identity = WindowsIdentity.GetCurrent();

            var name = Process.GetCurrentProcess().ProcessName;
            //var location = Assembly.GetEntryAssembly().Location;
            //var name = Assembly.GetEntryAssembly().GetName().Name;
            if (name is not null)
            {
                MessageBox.Show(name);
                var post = name.Replace('_', '/');

                await WindowsIdentity.RunImpersonated(identity.AccessToken, async () =>
                {
                    var endpoint = "LiveDraw/" + post;
                    try
                    {
                        var response = await client.PostAsync(endpoint, null);
                        if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            var msg = $"Failed to POST to {client.BaseAddress.ToString()}{endpoint}. Response: {response.StatusCode}.";
                            MessageBox.Show(msg);
                        }
                    }
                    catch (HttpRequestException hre)
                    {
                        MessageBox.Show(hre.Message);
                    }
                });
            }
        }
    }
}