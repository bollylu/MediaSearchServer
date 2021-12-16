using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

using MovieSearchClientServices;
using BLTools.Diagnostic.Logging;
using System.Net;
using BLTools;
using System.Linq;

namespace MovieSearchClient;

public class Program {

  public const string DEFAULT_EXTERNAL_API_ADDRESS = "https://mediasearchapi.sharenet.be/api/";
  public const string DEFAULT_API_ADDRESS = "http://mediasearchapi.sharenet.priv/api/";

  public static async Task Main(string[] args) {

    var builder = WebAssemblyHostBuilder.CreateDefault(args);

    builder.Services.AddSingleton<ILogger, TConsoleLogger>();
    builder.Services.AddSingleton<TImageCache>();

    //await Task.Delay(60000);
    IMovieService MovieService = new TMovieService(DEFAULT_API_ADDRESS, new TImageCache());

    builder.Services.AddSingleton(MovieService);


    builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();

    builder.RootComponents.Add<App>("#app");
    //builder.RootComponents.Add<HeadOutlet>("head::after");

    await builder.Build().RunAsync();

  }

}

