namespace MediaSearch.Models;
public class TListWithPrincipalJsonConverter<T> : JsonConverter<TListWithPrincipal<T>>, ILoggable {
  public ILogger Logger { get; set; } = GlobalSettings.LoggerPool.GetLogger<TFilterJsonConverter>();

  public override TListWithPrincipal<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    throw new NotImplementedException();
  }

  public override void Write(Utf8JsonWriter writer, TListWithPrincipal<T> value, JsonSerializerOptions options) {
    throw new NotImplementedException();
  }
}
