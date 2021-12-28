using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Microsoft.Net.Http.Headers;

namespace MediaSearch.Server.Support;

public class TMovieOutputFormatter : TextOutputFormatter {

  public TMovieOutputFormatter() {
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

    SupportedEncodings.Add(Encoding.UTF8);
    SupportedEncodings.Add(Encoding.Unicode);
  }

  protected override bool CanWriteType(Type type) {
    return typeof(TMovie).IsAssignableFrom(type);
  }

  public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
    HttpContext httpContext = context.HttpContext;

    TMovie Movie = context.Object as TMovie;
    await httpContext.Response.WriteAsync(Movie.ToJson());
  }
  
}
