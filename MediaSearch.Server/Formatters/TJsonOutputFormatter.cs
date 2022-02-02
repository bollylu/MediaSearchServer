using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;

using Microsoft.Net.Http.Headers;

namespace MediaSearch.Server.Support;

public class TJsonOutputFormatter : TextOutputFormatter {

  public TJsonOutputFormatter() {
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

    SupportedEncodings.Add(Encoding.UTF8);
    SupportedEncodings.Add(Encoding.Unicode);
  }

  protected override bool CanWriteType(Type? type) {
    return typeof(IJson).IsAssignableFrom(type);
  }

  public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
    HttpContext httpContext = context.HttpContext;

    IJson? JsonItem = context.Object as IJson;
    if (JsonItem is not null) {
      await httpContext.Response.WriteAsync(JsonItem.ToJson());
    }
  }
  
}
