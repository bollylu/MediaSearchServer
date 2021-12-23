using BLTools.Text;

using System.Drawing;
using System.Drawing.Imaging;

namespace MediaSearch.Models;

/// <summary>
/// Implement a movie
/// </summary>
public class TMovie : AMovie {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMovie() : base() { }
  public TMovie(IMovie movie) : base(movie) { }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- ToJson --------------------------------------------
  public override string ToJson() {

    return ToJson(new JsonWriterOptions());

  }

  public override string ToJson(JsonWriterOptions options) {

    using (MemoryStream Utf8JsonStream = new()) {
      using (Utf8JsonWriter Writer = new Utf8JsonWriter(Utf8JsonStream, options)) {

        Writer.WriteStartObject();

        Writer.WriteString(nameof(IMovie.Id), Id);
        Writer.WriteString(nameof(IMovie.Name), Name);
        Writer.WriteString(nameof(IMovie.Group), Group);
        Writer.WriteString(nameof(IMovie.StoragePath), StoragePath);
        Writer.WriteString(nameof(IMovie.FileName), FileName);
        Writer.WriteString(nameof(IMovie.FileExtension), FileExtension);
        Writer.WriteNumber(nameof(IMovie.Size), Size);
        Writer.WriteNumber(nameof(IMovie.OutputYear), OutputYear);
        Writer.WriteString(nameof(IMovie.DateAdded), $"{DateAdded.Year:0000}{DateAdded.Month:00}{DateAdded.Day:00}");

        Writer.WriteStartArray(nameof(IMovie.AltNames));
        foreach (KeyValuePair<string, string> AltNameItem in AltNames) {
          Writer.WriteStringValue($"{AltNameItem.Key}:{AltNameItem.Value}");
        }
        Writer.WriteEndArray();

        Writer.WriteStartArray(nameof(IMovie.Tags));
        foreach (string TagItem in Tags) {
          Writer.WriteStringValue(TagItem.Replace("[","").Replace("]",""));
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
    #region === Validate parameters ===
    if (string.IsNullOrWhiteSpace(source)) {
      throw new JsonException("Json movie source is null");
    }
    #endregion === Validate parameters ===

    try {
      JsonDocument JsonMovie = JsonDocument.Parse(source);
      JsonElement Root = JsonMovie.RootElement;

      //LogDebugEx(Root.GetRawText().BoxFixedWidth("RawText", 80, TextBox.EStringAlignment.Left));

      FileName = Root.GetPropertyEx(nameof(IMovie.FileName)).GetString();
      Name = Root.GetPropertyEx(nameof(IMovie.Name)).GetString();
      Group = Root.GetPropertyEx(nameof(IMovie.Group)).GetString();
      StoragePath = Root.GetPropertyEx(nameof(IMovie.StoragePath)).GetString();
      FileExtension = Root.GetPropertyEx(nameof(IMovie.FileExtension)).GetString();
      Size = Root.GetPropertyEx(nameof(IMovie.Size)).GetInt64();
      OutputYear = Root.GetPropertyEx(nameof(IMovie.OutputYear)).GetInt32();

      foreach (JsonElement AltNameItem in Root.GetPropertyEx(nameof(IMovie.AltNames)).EnumerateArray()) {
        string Key = AltNameItem.GetString().Before(':');
        string Value = AltNameItem.GetString().After(':');
        AltNames.Add(Key, Value);
      }

      foreach (JsonElement TagItem in Root.GetPropertyEx(nameof(IMovie.Tags)).EnumerateArray()) {
        //LogDebugEx(TagItem.GetRawText().BoxFixedWidth("TagItem",80, TextBox.EStringAlignment.Left));
        Tags.Add(TagItem.GetString());
      }

      string AddedDateString = Root.GetPropertyEx(nameof(IMovie.DateAdded)).GetString();
      //LogDebugEx(AddedDateString.BoxFixedWidth("Added", 20, TextBox.EStringAlignment.Left));
      DateAdded = new DateOnly(int.Parse(AddedDateString.Left(4)), int.Parse(AddedDateString.Substring(4, 2)), int.Parse(AddedDateString.Right(2)));

    } catch (Exception ex) {
      LogError($"Unable to parse json : {ex.Message}");
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
