using MediaSearch.Client;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

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

TApiServer? ApiServer = null;
foreach(string ApiServerAddressItem in ApiServerAdresses) {
  ApiServer = new TApiServer(ApiServerAddressItem);
  ApiServer.SetLogger(MediaSearch.Client.GlobalSettings.GlobalLogger);
  ApiServer.Logger.SeverityLimit = ESeverity.DebugEx;
  using (CancellationTokenSource Timeout = new CancellationTokenSource(5000)) {
    if (await ApiServer.ProbeServerAsync(Timeout.Token)) {
      break;
    } else {
      ApiServer = null;
    }
  }
}
if (ApiServer is null) {
  MediaSearch.Client.GlobalSettings.GlobalLogger.LogFatal("Missing api server");
  return;
} else {
  MediaSearch.Client.GlobalSettings.GlobalLogger.Log($"ApiServer={ApiServer}");
}

IMovieService MovieService = new TMovieService() { ApiServer = ApiServer };
builder.Services.AddSingleton(MovieService);

IAboutService AboutService = new TAboutService() { ApiServer = ApiServer };
builder.Services.AddSingleton(AboutService);

builder.Services.AddSingleton<IBusService<IFilter>, TBusService<IFilter>>();
builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
