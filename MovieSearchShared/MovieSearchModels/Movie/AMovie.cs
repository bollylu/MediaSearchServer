using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace MovieSearch.Models {
  public abstract class AMovie : ALoggable, IMovie {

    public string Storage { get; set; }
    public string LocalPath { get; set; }
    public string LocalName { get; set; }

    public List<string> AltNames { get; set; } = new();

    public string Group { get; set; }
    public long Size { get; set; }
    public int OutputYear { get; set; }
    public List<string> Tags { get; set; } = new();

    public virtual Task<byte[]> GetPicture(int timeout = 5000) {
      throw new NotImplementedException();
    }
    public virtual Task<string> GetPicture64(int timeout = 5000) {
      throw new NotImplementedException();
    }

    public static Lazy<string> PictureMissing => new Lazy<string>($"Pictures{Path.DirectorySeparatorChar}missing.jpg");

    protected static byte[] _PictureMissingBytes => TSupport.GetPicture(PictureMissing.Value);

    protected AMovie() { }
    protected AMovie(IMovie movie) {
      Storage = movie.Storage;
      LocalPath = movie.LocalPath;
      LocalName = movie.LocalName;
      Size = movie.Size;
      OutputYear = movie.OutputYear;
      Group = movie.Group;
      AltNames.AddRange(movie.AltNames);
      Tags.AddRange(movie.Tags);
    }

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendLine($"{nameof(Storage)} = {Storage}");
      RetVal.AppendLine($"{nameof(LocalPath)} = {LocalPath}");
      RetVal.AppendLine($"{nameof(LocalName)} = {LocalName}");
      RetVal.AppendLine($"{nameof(Group)} = {Group}");
      RetVal.AppendLine($"{nameof(Size)} = {Size}");
      RetVal.AppendLine($"{nameof(OutputYear)} = {OutputYear}");
      if (AltNames.Any()) {
        RetVal.AppendLine("Alt. names");
        foreach (string NameItem in AltNames) {
          RetVal.AppendLine($"|- {NameItem}");
        }
      } else {
        RetVal.AppendLine($"{nameof(AltNames)} is empty");
      }

      if (Tags.Any()) {
        RetVal.AppendLine("Tags");
        foreach (string TagItem in Tags) {
          RetVal.AppendLine($"|- {TagItem}");
        }
      } else {
        RetVal.AppendLine($"{nameof(Tags)} is empty");
      }

      return RetVal.ToString();
    }

    public abstract string ToJson();
    public abstract string ToJson(JsonWriterOptions options);

    public abstract IMovie ParseJson(string source);
    public abstract IMovie ParseJson(JsonElement source);
  }
}
