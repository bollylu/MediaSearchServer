using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MediaSearch.Models;

using MediaSearch.Client.Services;
using BLTools.Text;

namespace MediaSearch.Client.Test {
  [TestClass]
  public class ClientTests {

    public const int TIMEOUT_IN_MS = 10000;

    [TestMethod]
    public async Task Connect_GetHttpStatus() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = "movie/getWithFilter";
          TFilter Filter = TFilter.Empty;
          Console.WriteLine(Filter.ToString().Box("Filter"));
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {

            JsonContent Content = JsonContent.Create(Filter);
            HttpResponseMessage Result = await Client.PostAsync(RequestUrl, Content, Timeout.Token).ConfigureAwait(false);
            IMoviesPage Target = TMoviesPage.FromJson(await Result.Content.ReadAsStringAsync().ConfigureAwait(false));

            Console.WriteLine(Result.ReasonPhrase);
            Console.WriteLine(Target.ToString());

            Assert.IsNotNull(Result);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
          Assert.Fail(ex.Message);
        }
      }
    }

    [TestMethod]
    public async Task Connect_GetListOfMovies() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = $"movie/getWithFilter";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          TFilter Filter = TFilter.Empty;
          Console.WriteLine(Filter.ToString().Box("Filter"));
          JsonContent Content = JsonContent.Create(Filter);

          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
            HttpResponseMessage Result = await Client.PostAsync(RequestUrl, Content, Timeout.Token).ConfigureAwait(false);
            IMoviesPage Target = TMoviesPage.FromJson(await Result.Content.ReadAsStringAsync().ConfigureAwait(false));

            Console.WriteLine(Target.ToString());

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Target.Page);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
          Assert.Fail(ex.Message);
        }
      }
    }

    [TestMethod]
    public async Task Connect_GetListOfMoviesFiltered() {

      using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

        try {
          string RequestUrl = $"movie/getWithFilter";
          Console.WriteLine($"Requesting movies : {RequestUrl}");
          TFilter Filter = new TFilter() { Keywords = "the" };
          Console.WriteLine(Filter.ToString().Box("Filter"));
          JsonContent Content = JsonContent.Create(Filter);

          using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
            HttpResponseMessage Result = await Client.PostAsync(RequestUrl, Content, Timeout.Token).ConfigureAwait(false);
            IMoviesPage Target = TMoviesPage.FromJson(await Result.Content.ReadAsStringAsync().ConfigureAwait(false));

            Console.WriteLine(Target.ToString());

            Assert.IsNotNull(Result);
            Assert.AreEqual(1, Target.Page);
          }
        } catch (Exception ex) {
          Console.WriteLine($"Unable to get movies data : {ex.Message}");
          if (ex.InnerException is not null) {
            Console.WriteLine($"  Inner : {ex.InnerException.Message}");
          }
          Assert.Fail(ex.Message);
        }
      }
    }
  }
}
