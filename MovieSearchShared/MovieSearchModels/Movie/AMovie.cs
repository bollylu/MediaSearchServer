using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

namespace MovieSearchModels {
  public abstract class AMovie : AMedia, IMovie {

    #region --- Public properties ------------------------------------------------------------------------------
    public EMovieExtension Extension =>
      FileExtension switch {
        "avi" => EMovieExtension.AVI,
        "mkv" => EMovieExtension.MKV,
        "mp4" => EMovieExtension.MP4,
        "iso" => EMovieExtension.ISO,
        _ => EMovieExtension.Unknown,
      };

    public string Group { get; set; }
    public long Size { get; set; }
    public int OutputYear { get; set; }
    #endregion --- Public properties ---------------------------------------------------------------------------

    #region --- Picture --------------------------------------------
    public virtual Task<byte[]> GetPicture(int timeout = 5000) {
      throw new NotImplementedException();
    }
    public virtual Task<string> GetPicture64(int timeout = 5000) {
      throw new NotImplementedException();
    }

    public static byte[] PictureMissing
    {
      get
      {
        if (_PictureMissingBytes is null) {
          _PictureMissingBytes = TSupport.GetPicture("missing", ".jpg");
        }
        return _PictureMissingBytes;
      }
    }
    private static byte[] _PictureMissingBytes;
    #endregion --- Picture --------------------------------------------

    #region --- Constructor(s) ---------------------------------------------------------------------------------
    protected AMovie() { }
    protected AMovie(IMovie movie) : base(movie) {
      Size = movie.Size;
      OutputYear = movie.OutputYear;
      Group = movie.Group;
    }
    #endregion --- Constructor(s) ------------------------------------------------------------------------------

    public override string ToString() {
      StringBuilder RetVal = new StringBuilder(base.ToString());
      RetVal.AppendLine($"{nameof(Extension)} = {Extension}");
      RetVal.AppendLine($"{nameof(Group)} = {Group}");
      RetVal.AppendLine($"{nameof(Size)} = {Size}");
      RetVal.AppendLine($"{nameof(OutputYear)} = {OutputYear}");

      return RetVal.ToString();
    }

    public abstract string ToJson();
    public abstract string ToJson(JsonWriterOptions options);
    public abstract IJson ParseJson(string source);
    public abstract IJson ParseJson(JsonElement source);
  }
}
