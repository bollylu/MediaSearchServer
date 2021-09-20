using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Http;

using MovieSearchClient.Services;
using MovieSearch.Models;
using BLTools.Diagnostic.Logging;

namespace MovieSearchClient {
  public class Program {
    public static async Task Main(string[] args) {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);

      builder.Services.AddHttpClient<IMovieService, TMovieService>(client => 
        client.BaseAddress = new Uri("http://localhost:4567/api/")
      );

      builder.Services.AddSingleton<TImageCache>();
      builder.Services.AddSingleton<TMoviesConverter>(new TMoviesConverter(new TConsoleLogger()));
      builder.Services.AddSingleton<TMovieConverter>(new TMovieConverter(new TConsoleLogger()));

      builder.RootComponents.Add<App>("#app");

      builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();

      await builder.Build().RunAsync();
    }
  }
}
