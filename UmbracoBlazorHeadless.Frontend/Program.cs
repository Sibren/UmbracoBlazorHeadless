using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using UmbracoBlazorHeadless.Frontend.Umbraco;

namespace UmbracoBlazorHeadless.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            // this is not production ready. Move this to Azure Key Vault or similar!
#if DEBUG
            builder.Configuration.AddJsonFile("appsettings.Development.json", optional: true);
#endif
            var umbracoShizzle1 = builder.Configuration.GetValue<string>("AllowedHosts");
            var umbracoShizzle = builder.Configuration.GetSection("UmbracoApi").GetValue<string>("BaseUrl");
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
            .AddTransient<IUmbracoClient, UmbracoClient>()
            .AddHttpClient<IUmbracoApi, UmbracoApi>(
            (client, sp) =>
            {
                var config = sp.GetRequiredService<IConfiguration>();
                return new UmbracoApi(config["UmbracoApi:BaseUrl"] ?? throw new InvalidOperationException("Umbraco API url is not set."), client);
            });

            builder.Services.AddBlazoredLocalStorage();

            await builder.Build().RunAsync();
        }
    }
}
