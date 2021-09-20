using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace MovieSearch.Models {
  public class TMovieConverter : AJsonConverter<TMovie> {

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovieConverter() : base() { }

    public TMovieConverter(ILogger logger, int indent = 0) : base(logger, indent) { }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------


    public override TMovie Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

      if (reader.TokenType != JsonTokenType.StartObject) {
        throw new JsonException("Expected StartObject token");
      }

      int originalDepth = reader.CurrentDepth;

      TMovie RetVal = new TMovie();

      while (reader.GetNextToken(Logger)) {

        if (reader.TokenType == JsonTokenType.EndObject) {
          while (reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != originalDepth) {
            reader.GetNextToken(Logger);
          }
          break;
        }

        if (reader.TokenType == JsonTokenType.Comment) {
          continue;
        }

        if (reader.TokenType == JsonTokenType.PropertyName) {
          string PropertyName = reader.GetString();
          LogProperty(PropertyName);
          

          switch (PropertyName) {
            case nameof(TMovie.Storage):
              reader.GetNextToken(Logger);
              RetVal.Storage = reader.GetString();
              LogValue(RetVal.Storage);
              break;

            case nameof(TMovie.LocalPath):
              reader.GetNextToken(Logger);
              RetVal.LocalPath = reader.GetString();
              LogValue(RetVal.LocalPath);
              break;

            case nameof(TMovie.LocalName):
              reader.GetNextToken(Logger);
              RetVal.LocalName = reader.GetString();
              LogValue(RetVal.LocalName);
              break;

            case nameof(TMovie.Group):
              reader.GetNextToken(Logger);
              RetVal.Group = reader.GetString();
              LogValue(RetVal.Group);
              break;

            case nameof(TMovie.Size):
              reader.GetNextToken(Logger);
              RetVal.Size = reader.GetInt64();
              LogValue(RetVal.Size);
              break;

            case nameof(TMovie.OutputYear):
              reader.GetNextToken(Logger);
              RetVal.OutputYear = reader.GetInt32();
              LogValue(RetVal.OutputYear);
              break;

            case nameof(TMovie.AltNames):
              reader.GetNextToken(Logger);
              while (reader.GetNextToken(Logger)) {
                if (reader.TokenType == JsonTokenType.EndArray) {
                  break;
                }
                RetVal.AltNames.Add(reader.GetString());
                LogValue(RetVal.AltNames.Last());
              }
              break;

            case nameof(TMovie.Tags):
              reader.GetNextToken(Logger);
              while (reader.GetNextToken(Logger)) {
                if (reader.TokenType == JsonTokenType.EndArray) {
                  break;
                }
                RetVal.Tags.Add(reader.GetString());
                LogValue(RetVal.Tags.Last());
              }
              break;

          }
        }
      }

      while (reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != originalDepth) {
        reader.GetNextToken(Logger);
      }

      return RetVal;
    }

    public override void Write(Utf8JsonWriter writer, TMovie value, JsonSerializerOptions options) {
      writer.WriteStartObject();

      writer.WriteString(nameof(value.Storage), value.Storage);
      writer.WriteString(nameof(value.LocalPath), value.LocalPath);
      writer.WriteString(nameof(value.LocalName), value.LocalName);
      writer.WriteString(nameof(value.Group), value.Group);

      writer.WriteNumber(nameof(value.Size), value.Size);
      writer.WriteNumber(nameof(value.OutputYear), value.OutputYear);


      writer.WriteStartArray(nameof(value.AltNames));
      foreach (string AltNameItem in value.AltNames) {
        writer.WriteStringValue(AltNameItem);
      }
      writer.WriteEndArray();

      writer.WriteStartArray(nameof(value.Tags));
      foreach (string TagItem in value.Tags) {
        writer.WriteStringValue(TagItem);
      }
      writer.WriteEndArray();

      writer.WriteEndObject();
    }
  }
}
