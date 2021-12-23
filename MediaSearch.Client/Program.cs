using MediaSearch.Client;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


const string DEFAULT_EXTERNAL_API_ADDRESS = "https://mediasearchapi.sharenet.be/api/";
const string DEFAULT_API_ADDRESS = "http://mediasearchapi.sharenet.priv/api/";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Logging.SetMinimumLevel(LogLevel.Information);

builder.Services.AddSingleton<ILogger, TConsoleLogger>();
builder.Services.AddSingleton<TImageCache>();
builder.Services.AddLogging();

IMovieService MovieService = new TMovieService(DEFAULT_API_ADDRESS, new TImageCache(), new TConsoleLogger());
if (!await MovieService.ProbeApi()) {
  MovieService = new TMovieService(DEFAULT_EXTERNAL_API_ADDRESS, new TImageCache(), new TConsoleLogger());
}

builder.Services.AddSingleton(MovieService);

builder.Services.AddSingleton<IBusService<RFilter>, TBusService<RFilter>>();
builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();


builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
