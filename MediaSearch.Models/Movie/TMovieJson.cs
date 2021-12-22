using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLTools.Json;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public class TMovieJson : AMovie {

    public TMovieJson() { }
    public TMovieJson(IMovie movie) : base(movie) { }

    public JsonObject ToJson() {

      JsonObject RetVal = new();

      RetVal.Add(nameof(Storage), Storage);
      RetVal.Add(nameof(LocalPath), LocalPath);
      RetVal.Add(nameof(LocalName), LocalName);
      RetVal.Add(nameof(Group), Group);
      RetVal.Add(nameof(Size), Size);
      RetVal.Add(nameof(OutputYear), OutputYear);
      
      JsonArray JsonAltNames = new JsonArray();
      foreach (string NameItem in AltNames) {
        JsonAltNames.Add(new JsonString(NameItem));
      }
      RetVal.Add(nameof(AltNames), JsonAltNames);

      JsonArray JsonTags = new JsonArray();
      foreach (string TagItem in Tags) {
        JsonTags.Add(new JsonString(TagItem));
      }
      RetVal.Add(nameof(Tags), JsonTags);

      return RetVal;
    }
    public IMovie FromJson(JsonObject source) {

      TMovie RetVal = new();

      RetVal.Storage = source.SafeGetValueFirst<string>(nameof(Storage));
      RetVal.LocalPath = source.SafeGetValueFirst<string>(nameof(LocalPath));
      RetVal.LocalName = source.SafeGetValueFirst<string>(nameof(LocalName));
      RetVal.Group = source.SafeGetValueFirst<string>(nameof(Group));
      RetVal.Size = source.SafeGetValueFirst<long>(nameof(Size));
      RetVal.OutputYear = source.SafeGetValueFirst<int>(nameof(OutputYear));

      JsonArray JsonAltNames = source.SafeGetValueFirst<JsonArray>(nameof(AltNames));
      foreach(JsonString NameItem in JsonAltNames) {
        AltNames.Add(NameItem.Value);
      }

      JsonArray JsonTags = source.SafeGetValueFirst<JsonArray>(nameof(AltNames));
      foreach (JsonString TagItem in JsonTags) {
        Tags.Add(TagItem.Value);
      }

      return RetVal;
    }

  }
}
