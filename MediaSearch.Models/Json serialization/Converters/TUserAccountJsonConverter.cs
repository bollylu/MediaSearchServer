using BLTools.Text;

namespace MediaSearch.Models;
public class TUserAccountJsonConverter : JsonConverter<TUserAccount>, IMediaSearchLoggable<TUserAccountJsonConverter> {
  public IMediaSearchLogger<TUserAccountJsonConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger <TUserAccountJsonConverter>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserAccountJsonConverter() {}
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(TUserAccount).IsAssignableFrom(typeToConvert);
  }

  public override TUserAccount Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TUserAccount RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserAccountSecret", GlobalSettings.DEBUG_BOX_WIDTH));
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string? Property = reader.GetString();
          reader.Read();

          switch (Property) {

            case nameof(TUserAccount.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TUserAccount.Description):
              RetVal.Description = reader.GetString() ?? "";
              break;

            case nameof(TUserAccount.Secret):
              RetVal.Secret = JsonSerializer.Deserialize<TUserAccountSecret>(ref reader, options) ?? new TUserAccountSecret();
              break;

            default:
              Logger.LogWarning($"Invalid Json property name : {Property}", GetType().Name);
              break;
          }
        }
      }

      Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserAccountSecret", GlobalSettings.DEBUG_BOX_WIDTH));
      return RetVal;

    } catch (Exception ex) {
      Logger.LogError($"Problem during Json conversion : {ex.Message}");
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TUserAccount value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserAccount.Name), value.Name);
    writer.WriteString(nameof(TUserAccount.Description), value.Description);
    writer.WritePropertyName(nameof(TUserAccount.Secret));
    JsonSerializer.Serialize(writer, value.Secret, options);

    writer.WriteEndObject();
  }
}
