using Microsoft.AspNetCore.Http;

namespace MediaSearch.Server.Support;

public static class HttpRequestExtensions {

  public static string ListHeaders(this HttpRequest request, int width = GlobalSettings.DEBUG_BOX_WIDTH) {
    StringBuilder RetVal = new StringBuilder();
    foreach (var HeaderItem in request.Headers) {
      RetVal.AppendLine($"{HeaderItem.Key} = {HeaderItem.Value}");
    }
    return RetVal.ToString().BoxFixedWidth("Request headers", width);
  }

  public static Dictionary<string, string> GetHeaders(this HttpRequest request) {
    Dictionary<string, string> RetVal = new();
    foreach (var HeaderItem in request.Headers) {
      RetVal.Add(HeaderItem.Key, HeaderItem.Value);
    }
    return RetVal;
  }

  public static string ListHeaders(this HttpResponse response, int width = GlobalSettings.DEBUG_BOX_WIDTH) {
    StringBuilder RetVal = new StringBuilder();
    foreach (var HeaderItem in response.Headers) {
      RetVal.AppendLine($"{HeaderItem.Key} = {HeaderItem.Value}");
    }
    return RetVal.ToString().BoxFixedWidth("Response headers", width);
  }

  public static Dictionary<string, string> GetHeaders(this HttpResponse response) {
    Dictionary<string, string> RetVal = new();
    foreach (var HeaderItem in response.Headers) {
      RetVal.Add(HeaderItem.Key, HeaderItem.Value);
    }
    return RetVal;
  }
}
