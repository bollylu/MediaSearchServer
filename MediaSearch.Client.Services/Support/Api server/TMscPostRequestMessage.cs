using System.Reflection.PortableExecutable;

namespace MediaSearch.Client.Services;

internal class TMscPostRequestMessage : HttpRequestMessage {

  public readonly static string X_MSC = $"MediaSearchClient {MediaSearch.Client.Services.GlobalSettings.ExecutingAbout.CurrentVersion}";

  public TMscPostRequestMessage(string uri) : base(HttpMethod.Post, uri) {
    Headers.Add("X-Msc", X_MSC);
  }

  public TMscPostRequestMessage() {
    Method = HttpMethod.Post;
    Headers.Add("X-Msc", X_MSC);
  }

}
