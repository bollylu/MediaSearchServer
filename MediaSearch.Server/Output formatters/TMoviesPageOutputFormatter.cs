﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace MediaSearch.Server.Support;

public class TMoviesPageOutputFormatter : TextOutputFormatter {

  public TMoviesPageOutputFormatter() {
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

    SupportedEncodings.Add(Encoding.UTF8);
    SupportedEncodings.Add(Encoding.Unicode);
  }

  protected override bool CanWriteType(Type type) {
    return typeof(TMoviesPage).IsAssignableFrom(type);
  }

  public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding) {
    HttpContext httpContext = context.HttpContext;

    TMoviesPage MoviesPage = context.Object as TMoviesPage;
    await httpContext.Response.WriteAsync(MoviesPage.ToJson());
  }
  
}
