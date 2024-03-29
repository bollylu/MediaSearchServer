﻿using static MediaSearch.Models.JsonConverterResources;

namespace MediaSearch.Models;

public class TMoviesPageJsonConverter : JsonConverter<TMoviesPage> {

  public IMediaSearchLogger<TMoviesPageJsonConverter> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMoviesPageJsonConverter>();

  public override bool CanConvert(Type typeToConvert) {
    return typeToConvert == typeof(TMoviesPage);
  }

  public override TMoviesPage Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

    if (reader.TokenType != JsonTokenType.StartObject) {
      throw new JsonException();
    }

    TMoviesPage RetVal = new TMoviesPage();

    try {
      while (reader.Read()) {

        JsonTokenType TokenType = reader.TokenType;

        if (TokenType == JsonTokenType.EndObject) {
          return RetVal;
        }

        if (TokenType == JsonTokenType.PropertyName) {

          string Property = reader.GetString() ?? "";
          reader.Read();

          switch (Property) {

            case nameof(TMoviesPage.Name):
              RetVal.Name = reader.GetString() ?? "";
              break;

            case nameof(TMoviesPage.Source):
              RetVal.Source = reader.GetString() ?? "";
              break;

            case nameof(TMoviesPage.Page):
              RetVal.Page = reader.GetInt32();
              break;

            case nameof(TMoviesPage.AvailablePages):
              RetVal.AvailablePages = reader.GetInt32();
              break;

            case nameof(TMoviesPage.AvailableMovies):
              RetVal.AvailableMovies = reader.GetInt32();
              break;

            case nameof(TMoviesPage.Movies):
              while (reader.Read() && reader.TokenType != JsonTokenType.EndArray) {
                TMovie? MovieItem = JsonSerializer.Deserialize<TMovie>(ref reader, options);
                if (MovieItem is not null) {
                  RetVal.Movies.Add(MovieItem);
                }
              }
              break;

            default:
              Logger.LogWarningBox(ERROR_INVALID_PROPERTY, Property);
              break;
          }
        }
      }

      return RetVal;

    } catch (Exception ex) {
      Logger.LogErrorBox(ERROR_CONVERSION, ex);
      throw;
    }

  }


  public override void Write(Utf8JsonWriter writer, TMoviesPage value, JsonSerializerOptions options) {
    if (value is null) {
      writer.WriteNullValue();
      return;
    }
    writer.WriteStartObject();

    writer.WriteString(nameof(TMoviesPage.Name), value.Name);
    writer.WriteString(nameof(TMoviesPage.Source), value.Source);
    writer.WriteNumber(nameof(TMoviesPage.Page), value.Page);
    writer.WriteNumber(nameof(TMoviesPage.AvailablePages), value.AvailablePages);
    writer.WriteNumber(nameof(TMoviesPage.AvailableMovies), value.AvailableMovies);

    writer.WriteStartArray(nameof(TMoviesPage.Movies));
    foreach (TMovie Movie in value.Movies) {
      JsonSerializer.Serialize(writer, Movie, options);
    }
    writer.WriteEndArray();

    writer.WriteEndObject();
  }
}
