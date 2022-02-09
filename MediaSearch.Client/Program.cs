using MediaSearch.Client;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

await GlobalProperties.About.Initialize();

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddSingleton<ILogger, TConsoleLogger>();
builder.Services.AddSingleton<TImageCache>();
builder.Services.AddLogging();

List<string> ApiServerAdresses = new List<string>() {
  "http://localhost:4567/api/",
  "http://mediasearchapi.sharenet.priv/api/",
  "https://mediasearchapi.sharenet.be/api/"
};
IApiServer? ApiServer = null;
foreach(string ApiServerAddressItem in ApiServerAdresses) {
  ApiServer = new TApiServer(ApiServerAddressItem);
  using (CancellationTokenSource Timeout = new CancellationTokenSource(5000)) {
    if (await ApiServer.ProbeServerAsync(Timeout.Token)) {
      break;
    } else {
      ApiServer = null;
    }
  }
}
if (ApiServer is null) {
  new TConsoleLogger().LogFatal("Missing api server");
  return;
}

IMovieService MovieService = new TMovieService() { ApiServer = ApiServer };

builder.Services.AddSingleton(MovieService);

builder.Services.AddSingleton<IBusService<IFilter>, TBusService<IFilter>>();
builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
