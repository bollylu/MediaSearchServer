using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BLTools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearch.Models;

using MovieSearchClient.Services;

namespace MovieSearchClientTest {
  [TestClass]
  public class ClientTests {

    public const int TIMEOUT = 100000;

    [TestMethod]
    public async Task Connect_GetHttpStatus() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = "movie";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT)) {
            JsonSerializerOptions Options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            IMovies Result = await Client.GetFromJsonAsync<TMovies>(RequestUrl, Options, Timeout.Token).ConfigureAwait(false);

            Console.WriteLine(Result.ToString());
            Console.WriteLine($"Result : {Result.Source} - {Result.Movies.Count}");

            Assert.IsNotNull(Result);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
        }
      }
    }

    [TestMethod]
    public async Task Connect_GetListOfMovies() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = $"movie?filter=";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT)) {
            JsonSerializerOptions Options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            IMovies Result = await Client.GetFromJsonAsync<TMovies>(RequestUrl, Options, Timeout.Token).ConfigureAwait(false);

            Console.WriteLine(Result.ToString());
            Console.WriteLine($"Result : {Result.Source} - {Result.Movies.Count}");

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Result.Page);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
        }
      }
    }

    [TestMethod]
    public async Task Connect_GetListOfMoviesFiltered() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = $"movie?filter=the";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT)) {
            JsonSerializerOptions Options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            IMovies Result = await Client.GetFromJsonAsync<TMovies>(RequestUrl, Options, Timeout.Token).ConfigureAwait(false);

            Console.WriteLine(Result.ToString());
            Console.WriteLine($"Result : {Result.Source} - {Result.Movies.Count}");

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Result.Page);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
        }
      }
    }
  }
}
