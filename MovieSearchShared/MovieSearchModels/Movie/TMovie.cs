using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

using BLTools;
using BLTools.Diagnostic.Logging;

namespace MovieSearch.Models {

  /// <summary>
  /// Implement a movie
  /// </summary>
  public class TMovie : AMovie {

    public TMovie() : base() { }
    public TMovie(IMovie movie) : base(movie) { }

    public override async Task<byte[]> GetPicture(int timeout = 5000) {

      string FullFileName = Path.Combine(Storage, LocalPath, IMovie.DEFAULT_PICTURE_NAME);

      if (!File.Exists(FullFileName)) {
        return _PictureMissingBytes;
      }

      using (CancellationTokenSource TimeOut = new CancellationTokenSource(timeout)) {
        try {
          using (Image FolderJpg = Image.FromStream((await File.ReadAllBytesAsync(FullFileName, TimeOut.Token)).ToStream())) {
            using (Bitmap ResizedPicture = new Bitmap(FolderJpg, 128, 160)) {
              using (MemoryStream OutputStream = new()) {
                ResizedPicture.Save(OutputStream, ImageFormat.Jpeg);
                return OutputStream.ToArray();
              }
            }
          }
        } catch (Exception ex) {
          LogError($"Unable to get picture {FullFileName} : {ex.Message}");
          return _PictureMissingBytes;
        }
      }
    }

    public override async Task<string> GetPicture64(int timeout = 5000) {

      byte[] PictureBytes = await GetPicture(timeout);
      return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
    }

    #region --- ToJson --------------------------------------------
    public override string ToJson() {

      return ToJson(new JsonWriterOptions());

    }

    public override string ToJson(JsonWriterOptions options) {

      using (MemoryStream Utf8JsonStream = new()) {
        using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

          Writer.WriteStartObject();

          Writer.WriteString(nameof(IMovie.Storage), Storage);
          Writer.WriteString(nameof(IMovie.LocalPath), LocalPath);
          Writer.WriteString(nameof(IMovie.LocalName), LocalName);
          Writer.WriteNumber(nameof(IMovie.Size), Size);
          Writer.WriteNumber(nameof(IMovie.OutputYear), OutputYear);

          Writer.WriteStartArray(nameof(IMovie.AltNames));
          foreach (string AltNameItem in AltNames) {
            Writer.WriteStringValue(AltNameItem);
          }
          Writer.WriteEndArray();

          Writer.WriteStartArray(nameof(IMovie.Tags));
          foreach (string TagItem in Tags) {
            Writer.WriteStringValue(TagItem);
          }
          Writer.WriteEndArray();

          Writer.WriteEndObject();
        }

        return Encoding.UTF8.GetString(Utf8JsonStream.ToArray());
      }
    }
    #endregion --- ToJson --------------------------------------------

    #region --- ParseJson --------------------------------------------
    public override IMovie ParseJson(JsonElement source) {
      return ParseJson(source.GetRawText());
    }

    public override IMovie ParseJson(string source) {
      if (string.IsNullOrWhiteSpace(source)) {
        throw new JsonException("Json movie source is null");
      }

      JsonDocument JsonMovie = JsonDocument.Parse(source);
      JsonElement Root = JsonMovie.RootElement;

      Storage = Root.GetPropertyEx(nameof(IMovie.Storage)).GetString();
      LocalPath = Root.GetPropertyEx(nameof(IMovie.LocalPath)).GetString();
      LocalName = Root.GetPropertyEx(nameof(IMovie.LocalName)).GetString();
      Size = Root.GetPropertyEx(nameof(IMovie.Size)).GetInt64();
      OutputYear = Root.GetPropertyEx(nameof(IMovie.OutputYear)).GetInt32();

      foreach (JsonElement AltNameItem in Root.GetPropertyEx(nameof(IMovie.AltNames)).EnumerateArray()) {
        AltNames.Add(AltNameItem.GetString());
      }

      foreach (JsonElement TagItem in Root.GetPropertyEx(nameof(IMovie.Tags)).EnumerateArray()) {
        Tags.Add(TagItem.GetString());
      }
      return this;
    }
    #endregion --- ParseJson --------------------------------------------

    #region --- Static FromJson --------------------------------------------
    public static IMovie FromJson(JsonElement source) {
      if (source.ValueKind != JsonValueKind.Object) {
        throw new JsonException("Json movie source is not an object");
      }

      return FromJson(source.GetRawText());
    }

    public static IMovie FromJson(string source) {
      TMovie RetVal = new();
      return RetVal.ParseJson(source);
    } 
    #endregion --- Static FromJson --------------------------------------------

  }
}
