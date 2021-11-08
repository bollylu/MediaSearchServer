using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BLTools;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearchModels;

using MovieSearchClientServices;
using MovieSearchClient;

namespace MovieSearchClientTest {
  [TestClass]
  public class ClientTests {

    public const int TIMEOUT_IN_MS = 10000;

    [TestInitialize]

    [TestMethod]
    public async Task Connect_GetHttpStatus() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = "movie";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {

            HttpResponseMessage Result = await Client.GetAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
            IMovies Target = TMovies.FromJson(await Result.Content.ReadAsStringAsync().ConfigureAwait(false));


            Console.WriteLine(Result.ReasonPhrase);
            Console.WriteLine(Target.ToString());
            Console.WriteLine($"Result : {Target.Source} - {Target.Movies.Count}");

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
    public async Task Connect_GetHttpStatusEx() {

      using (THttpClientEx Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = "movie";
          Console.WriteLine($"Requesting movies : {RequestUrl}");

          string Result = await Client.GetStringAsync(RequestUrl, TIMEOUT_IN_MS).ConfigureAwait(false);

          if (Client.LastResponse.IsSuccessStatusCode) {
            IMovies Target = TMovies.FromJson(Result);

            Console.WriteLine(Result);
            Console.WriteLine(Target.ToString());
            Console.WriteLine($"Result : {Target.Source} - {Target.Movies.Count}");

            Assert.IsNotNull(Result);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
          Console.WriteLine($"Status : {Client.LastResponse}");
        }
      }
    }

    [TestMethod]
    public async Task Connect_GetListOfMovies() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = $"movie?filter=";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
            string Result = await Client.GetStringAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
            IMovies Target = TMovies.FromJson(Result);

            Console.WriteLine(Target.ToString());
            Console.WriteLine($"Result : {Target.Source} - {Target.Movies.Count}");

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Target.Page);
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
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
            string Result = await Client.GetStringAsync(RequestUrl, Timeout.Token).ConfigureAwait(false);
            IMovies Target = TMovies.FromJson(Result);

            Console.WriteLine(Target.ToString());
            Console.WriteLine($"Result : {Target.Source} - {Target.Movies.Count}");

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Target.Page);
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
