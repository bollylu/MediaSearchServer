using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace MediaSearch.Server;

public class TJsonInputFormatter : TextInputFormatter {

  public TJsonInputFormatter() {
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/json"));
    SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/*+json"));

    SupportedEncodings.Add(Encoding.UTF8);
    SupportedEncodings.Add(Encoding.Unicode);
  }

  protected override bool CanReadType(Type type) {
    return typeof(IJson).IsAssignableFrom(type);
  }


  public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context, Encoding encoding) {
    StreamReader Reader = new StreamReader(context.HttpContext.Request.Body);
    string Body = await Reader.ReadToEndAsync();
    switch (context.Metadata.ModelType.Name) {
      case nameof(TFilter):
        return await InputFormatterResult.SuccessAsync(IJson<TFilter>.FromJson(Body));
      case nameof(TUserAccountSecret):
        return await InputFormatterResult.SuccessAsync(IJson<TUserAccountSecret>.FromJson(Body));
      case nameof(TUserToken):
        return await InputFormatterResult.SuccessAsync(IJson<TUserToken>.FromJson(Body));
      default:
        return await InputFormatterResult.FailureAsync();
    }
  }
}
