using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using MovieSearchModels;

using Microsoft.Net.Http.Headers;

namespace MovieSearchServer;

public class TMovieOutputFormatter : TextOutputFormatter {

  public TMovieOutputFormatter() {
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

    SupportedEncodings.Add(Encoding.UTF8);
    SupportedEncodings.Add(Encoding.Unicode);
  }

  protected override bool CanWriteType(Type type) {
    return typeof(IMovie).IsAssignableFrom(type);
  }

  public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
    HttpContext httpContext = context.HttpContext;

    IMovie Movie = context.Object as IMovie;
    await httpContext.Response.WriteAsync(Movie.ToJson());
  }
  
}
