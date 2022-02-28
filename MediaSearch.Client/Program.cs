using BLTools.Diagnostic.Logging;

using MediaSearch.Client;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

MediaSearch.Client.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerConsole() { SeverityLimit = ESeverity.Debug });
MediaSearch.Client.Services.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerConsole() { SeverityLimit = ESeverity.DebugEx });
MediaSearch.Models.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerConsole() { SeverityLimit = ESeverity.Debug });

IMediaSearchLogger<Program> Logger = MediaSearch.Client.GlobalSettings.LoggerPool.GetLogger<Program>();

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Logging.SetMinimumLevel(LogLevel.Information);

//builder.Services.AddSingleton<IMediaSearchLogger, AMediaSearchLogger>();
builder.Services.AddSingleton<TImageCache>();
builder.Services.AddLogging();

List<string> ApiServerAdresses = new List<string>() {
  "http://localhost:4567/api/",
  "http://mediasearchapi.sharenet.priv/api/",
  "https://mediasearchapi.sharenet.be/api/"
};

Logger.Log("Probing api server");

TApiServer? ApiServer = null;
foreach (string ApiServerAddressItem in ApiServerAdresses) {
  ApiServer = new TApiServer(ApiServerAddressItem);
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
  Logger.LogFatal("Missing api server");
  return;
} else {
  Logger.Log($"ApiServer={ApiServer}");
}

IMovieService MovieService = new TMovieService() { ApiServer = ApiServer };
builder.Services.AddSingleton(MovieService);

IAboutService AboutService = new TAboutService() { ApiServer = ApiServer };
builder.Services.AddSingleton(AboutService);

ILoginService LoginService = new TLoginService() { ApiServer = ApiServer };
builder.Services.AddSingleton(LoginService);

builder.Services.AddSingleton<IBusService<IFilter>, TBusService<IFilter>>();
builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
