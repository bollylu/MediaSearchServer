using BLTools.Text;

namespace MediaSearch.Models;
public class TUserTokenJsonConverter : JsonConverter<TUserToken>, IMediaSearchLoggable<TUserTokenJsonConverter> {
  public IMediaSearchLogger<TUserTokenJsonConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TUserTokenJsonConverter>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TUserTokenJsonConverter() { }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public override bool CanConvert(Type typeToConvert) {
    return typeof(TUserToken).IsAssignableFrom(typeToConvert);
  }

  public override TUserToken Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TUserToken RetVal = new();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserToken", GlobalSettings.DEBUG_BOX_WIDTH));
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string? Property = reader.GetString();
          reader.Read();

          switch (Property) {

            case nameof(TUserToken.TokenId):
              RetVal.TokenId = reader.GetString() ?? "";
              break;

            case nameof(TUserToken.Expiration):
              RetVal.Expiration = DateTime.Parse(reader.GetString() ?? DateTime.Now.ToDMYHMS());
              break;

            default:
              Logger.LogWarning($"Invalid Json property name : {Property}", GetType().Name);
              break;
          }
        }
      }

      Logger.LogDebug(RetVal.ToString().BoxFixedWidth("Converted UserToken", GlobalSettings.DEBUG_BOX_WIDTH));
      return RetVal;

    } catch (Exception ex) {
      Logger.LogError($"Problem during Json conversion : {ex.Message}");
      throw;
    }
  }

  public override void Write(Utf8JsonWriter writer, TUserToken value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TUserToken.TokenId), value.TokenId);
    writer.WriteString(nameof(TUserToken.Expiration), value.Expiration.ToDMYHMS());

    writer.WriteEndObject();
  }
}
