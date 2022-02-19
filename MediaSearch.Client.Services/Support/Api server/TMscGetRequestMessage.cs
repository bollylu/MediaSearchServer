namespace MediaSearch.Client.Services;

internal class TMscGetRequestMessage : HttpRequestMessage {

  public readonly static string X_MSC = $"MediaSearchClient {MediaSearch.Client.Services.GlobalSettings.ExecutingAbout.CurrentVersion}";

  public TMscGetRequestMessage(string uri) : base(HttpMethod.Get, uri) {
    Headers.Add("X-Msc", X_MSC);
  }

  public TMscGetRequestMessage() {
    Method = HttpMethod.Get;
    Headers.Add("X-Msc", X_MSC);
  }
}
