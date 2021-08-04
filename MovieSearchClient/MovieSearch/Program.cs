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

using MovieSearch.Models;
using MovieSearch.Services;
using MovieSearch.Client.Services;

namespace MovieSearch {
  public class Program {
    public static async Task Main(string[] args) {
      var builder = WebAssemblyHostBuilder.CreateDefault(args);

      builder.Services.AddHttpClient<IMovieService, TMovieService>(client => 
        client.BaseAddress = new Uri("https://lucwks7.sharenet.priv:4567/api/")
      );

      builder.Services.AddSingleton<TImageCache>();

      builder.RootComponents.Add<App>("#app");

      //builder.Services.AddScoped<IMovieService>(sp => new TMovieService(new HttpClient() { BaseAddress = new Uri("http://multimedia.sharenet.priv:4567") }));
      builder.Services.AddSingleton<IBusService<string>, TBusService<string>>();

      await builder.Build().RunAsync();
    }
  }
}
