using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace MovieSearch.Models {
  public class TMoviesConverter : AJsonConverter<TMovies> {

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMoviesConverter() : base() { }

    public TMoviesConverter(ILogger logger, int indent = 0) : base(logger, indent) { }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override TMovies Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
      if (reader.TokenType != JsonTokenType.StartObject) {
        throw new JsonException("Expected StartObject token");
      }

      int originalDepth = reader.CurrentDepth;

      TMovies RetVal = new();

      while (reader.GetNextToken(Logger)) {

        if (reader.TokenType == JsonTokenType.EndObject) {
          //reader.GetNextToken(Logger);
          //while (reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != originalDepth) {
          //  reader.GetNextToken(Logger);
          //}
          break;
        }

        if (reader.TokenType == JsonTokenType.Comment) {
          continue;
        }

        if (reader.TokenType == JsonTokenType.PropertyName) {

          string PropertyName = reader.GetString();
          LogProperty(PropertyName);

          switch (PropertyName) {

            case nameof(IMovies.Name):
              reader.GetNextToken(Logger);
              RetVal.Name = reader.GetString();
              LogValue(reader.GetString());
              break;

            case nameof(IMovies.Source):
              reader.GetNextToken(Logger);
              RetVal.Source = reader.GetString();
              LogValue(reader.GetString());
              break;

            case nameof(IMovies.Page):
              reader.GetNextToken(Logger);
              RetVal.Page = reader.GetInt32();
              LogValue(reader.GetInt32());
              break;

            case nameof(IMovies.AvailablePages):
              reader.GetNextToken(Logger);
              RetVal.AvailablePages = reader.GetInt32();
              LogValue(reader.GetInt32());
              break;

            case nameof(IMovies.Movies):
              
              JsonSerializerOptions MovieOptions = new() { 
                PropertyNameCaseInsensitive = true,
                Converters = {
                    new TMovieConverter(Logger, _Indent+2)
                  }
              };

              while (reader.GetNextToken(Logger) && reader.TokenType != JsonTokenType.EndArray) {

                if (reader.TokenType == JsonTokenType.StartArray) {
                  reader.GetNextToken(Logger); 
                }

                TMovie Movie = JsonSerializer.Deserialize<TMovie>(ref reader, MovieOptions);
                RetVal.Movies.Add(Movie);

                //while (reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != originalDepth) {
                //  reader.GetNextToken(Logger);
                //}
                //LogValue("Deserialized.");
                //LogValue(Movie);
                
              }
              break;

          }
        }
      }

      return RetVal;
    }


    public override void Write(Utf8JsonWriter writer, TMovies value, JsonSerializerOptions options) {
      throw new NotImplementedException();
    }


  }
}
