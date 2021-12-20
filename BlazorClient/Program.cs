using BlazorClient;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using MovieSearchClientModels;

using MovieSearchClientServices;

const string DEFAULT_EXTERNAL_API_ADDRESS = "https://mediasearchapi.sharenet.be/api/";
const string DEFAULT_API_ADDRESS = "http://mediasearchapi.sharenet.priv/api/";

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<ILogger, TTraceLogger>();
builder.Services.AddSingleton<TImageCache>();


IMovieService MovieService = new TMovieService(DEFAULT_API_ADDRESS, new TImageCache());
if (!await MovieService.ProbeApi()) {
  MovieService = new TMovieService(DEFAULT_EXTERNAL_API_ADDRESS, new TImageCache());
}

builder.Services.AddSingleton(MovieService);

builder.Services.AddSingleton<IBusService<SearchFilter>, TBusService<SearchFilter>>();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
