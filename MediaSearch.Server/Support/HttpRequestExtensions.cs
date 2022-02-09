using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace MediaSearch.Server.Support;

public static class HttpRequestExtensions {

  public static string ListHeaders(this HttpRequest request, int width = TGlobalSettings.DEBUG_BOX_WIDTH) {
    StringBuilder RetVal = new StringBuilder();
    foreach (var HeaderItem in request.Headers) {
      RetVal.AppendLine($"{HeaderItem.Key} = {HeaderItem.Value}");
    }
    return RetVal.ToString().BoxFixedWidth("Request headers", width);
  }

  public static string ListHeaders(this HttpResponse response, int width = TGlobalSettings.DEBUG_BOX_WIDTH) {
    StringBuilder RetVal = new StringBuilder();
    foreach (var HeaderItem in response.Headers) {
      RetVal.AppendLine($"{HeaderItem.Key} = {HeaderItem.Value}");
    }
    return RetVal.ToString().BoxFixedWidth("Response headers", width);
  }
}
