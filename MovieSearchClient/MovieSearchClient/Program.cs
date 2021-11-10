using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;

using MovieSearchClientServices;
using BLTools.Diagnostic.Logging;
using System.Net;
using BLTools;
using System.Linq;

namespace MovieSearchClient {
  public class Program {

    public const string DEFAULT_EXTERNAL_API_ADDRESS = "https://mediasearch.sharenet.be/api/";
    public const string DEFAULT_API_ADDRESS = "http://mediasearch.sharenet.priv/api/";

    public static async Task Main(string[] args) {

      var builder = WebAssemblyHostBuilder.CreateDefault(args);

      builder.Services.AddSingleton<ILogger, TConsoleLogger>();

      if ( _ProbeLocalServer() ) {

        builder.Services.AddSingleton<TApiServer>(new TApiServer() { BaseAddress = new Uri(DEFAULT_API_ADDRESS) });
        builder.Services.AddSingleton<THttpClientEx>(new THttpClientEx() { BaseAddress = new Uri(DEFAULT_API_ADDRESS) });
      } else {
        builder.Services.AddSingleton<TApiServer>(new TApiServer() { BaseAddress = new Uri(DEFAULT_EXTERNAL_API_ADDRESS) });
        builder.Services.AddSingleton<THttpClientEx>(new THttpClientEx() { BaseAddress = new Uri(DEFAULT_EXTERNAL_API_ADDRESS) });
      }

      builder.Services.AddSingleton<IMovieService, TMovieService>();

      //builder.Services.AddHttpClient<IMovieService, TMovieService>(client =>
      //  client.BaseAddress = new Uri(DEFAULT_API_ADDRESS)
      //) ;

      builder.Services.AddSingleton<TImageCache>();
      builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();

      builder.RootComponents.Add<App>("#app");

      await builder.Build().RunAsync();

    }

    private static bool _ProbeLocalServer() {
      using ( THttpClientEx Client = new() {
        BaseAddress = new Uri(DEFAULT_API_ADDRESS)
      } ) {

      }

      return true;
    }
  }
}
