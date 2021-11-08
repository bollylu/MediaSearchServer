using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

using BLTools;

namespace MovieSearchModels {

  /// <summary>
  /// Implement a movie
  /// </summary>
  public class TMovie : AMovie, IJson {

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    public TMovie() : base() { }
    public TMovie(IMovie movie) : base(movie) { } 
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    //public override async Task<byte[]> GetPicture(int timeout = 5000) {

    //  string FullFileName = Path.Combine(Storage, LocalPath, IMovie.DEFAULT_PICTURE_NAME);

    //  if (!File.Exists(FullFileName)) {
    //    return PictureMissing;
    //  }

    //  using (CancellationTokenSource TimeOut = new CancellationTokenSource(timeout)) {
    //    try {
    //      using (Image FolderJpg = Image.FromStream((await File.ReadAllBytesAsync(FullFileName, TimeOut.Token)).ToStream())) {
    //        using (Bitmap ResizedPicture = new Bitmap(FolderJpg, 128, 160)) {
    //          using (MemoryStream OutputStream = new()) {
    //            ResizedPicture.Save(OutputStream, ImageFormat.Jpeg);
    //            return OutputStream.ToArray();
    //          }
    //        }
    //      }
    //    } catch (Exception ex) {
    //      LogError($"Unable to get picture {FullFileName} : {ex.Message}");
    //      return PictureMissing;
    //    }
    //  }
    //}

    //public override async Task<string> GetPicture64(int timeout = 5000) {

    //  byte[] PictureBytes = await GetPicture(timeout);
    //  return $"data:image/jpg;base64, {Convert.ToBase64String(PictureBytes)}";
    //}

    #region --- ToJson --------------------------------------------
    public override string ToJson() {

      return ToJson(new JsonWriterOptions());

    }

    public override string ToJson(JsonWriterOptions options) {

      using (MemoryStream Utf8JsonStream = new()) {
        using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

          Writer.WriteStartObject();

          Writer.WriteString(nameof(IMovie.StorageRoot), StorageRoot);
          Writer.WriteString(nameof(IMovie.StoragePath), StoragePath);
          Writer.WriteString(nameof(IMovie.Name), Name);
          Writer.WriteString(nameof(IMovie.Group), Group);
          Writer.WriteString(nameof(IMovie.Filename), Filename);
          Writer.WriteString(nameof(IMovie.FileExtension), FileExtension);
          Writer.WriteNumber(nameof(IMovie.Size), Size);
          Writer.WriteNumber(nameof(IMovie.OutputYear), OutputYear);

          Writer.WriteStartArray(nameof(IMovie.AltNames));
          foreach (KeyValuePair<string, string> AltNameItem in AltNames) {
            Writer.WriteStringValue($"{AltNameItem.Key}:{AltNameItem.Value}");
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

      StorageRoot = Root.GetPropertyEx(nameof(IMovie.StorageRoot)).GetString();
      StoragePath = Root.GetPropertyEx(nameof(IMovie.StoragePath)).GetString();
      Filename = Root.GetPropertyEx(nameof(IMovie.Filename)).GetString();
      Name = Root.GetPropertyEx(nameof(IMovie.Name)).GetString();
      Group = Root.GetPropertyEx(nameof(IMovie.Group)).GetString();
      FileExtension = Root.GetPropertyEx(nameof(IMovie.FileExtension)).GetString();
      Size = Root.GetPropertyEx(nameof(IMovie.Size)).GetInt64();
      OutputYear = Root.GetPropertyEx(nameof(IMovie.OutputYear)).GetInt32();

      foreach (JsonElement AltNameItem in Root.GetPropertyEx(nameof(IMovie.AltNames)).EnumerateArray()) {
        string Key = AltNameItem.GetString().Before(':');
        string Value = AltNameItem.GetString().After(':');
        AltNames.Add(Key, Value);
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
