using System.Collections.Generic;
using System.Net;

using MediaSearch.Client.Services;

namespace MediaSearch.Client.Test;

[TestClass]
public class ClientTests {

  public const int TIMEOUT_IN_MS = 10000;

  [TestMethod]
  public async Task Connect_GetHttpStatus() {

    using (HttpClient Client = new() { BaseAddress = new Uri("http://localhost:4567/api/") }) {

      try {
        HttpRequestMessage Request = new();
        using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {

          HttpResponseMessage Result = await Client.SendAsync(Request, Timeout.Token).ConfigureAwait(false);

          TraceMessage($"Result {Result.StatusCode}", await Result.Content.ReadAsStringAsync());

          Assert.IsNotNull(Result);
          Assert.AreEqual(HttpStatusCode.OK, Result.StatusCode);
        }
      } catch (Exception ex) {
        TraceMessage("Unable to connect", ex.Message);
        if (ex.InnerException is not null) {
          TraceMessage("  Inner", ex.InnerException.Message);
        }
        Assert.Fail(ex.Message);
      }
    }
  }

  
}
