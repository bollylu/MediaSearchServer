using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public interface IMovie {

    /// <summary>
    /// The root of the path URI
    /// </summary>
    string Storage { get; }

    /// <summary>
    /// The path of the movie in the storage
    /// </summary>
    string LocalPath { get; }

    /// <summary>
    /// The name of the movie in local storage
    /// </summary>
    string LocalName { get; }

    /// <summary>
    /// A list of alternate names (0 <= count <= n)
    /// </summary>
    List<string> AltNames { get; }

    /// <summary>
    /// The group where the movie belongs
    /// Group components are separated by '/' (eg. Comédie/Aventure)
    /// </summary>
    string Group { get; }

    /// <summary>
    /// The movie size
    /// </summary>
    long Size { get; }

    /// <summary>
    /// The year when the movie was distributed on cinema/DTV/...
    /// </summary>
    int OutputYear { get; }
    
    /// <summary>
    /// A list of tags (0 <= count <= n)
    /// </summary>
    List<string> Tags { get; }

    #region --- Pictures --------------------------------------------
    /// <summary>
    /// Get the movie cover in bytes
    /// </summary>
    Task<byte[]> GetPicture(int timeout = 5000);

    /// <summary>
    /// Get the movie cover in base64
    /// </summary>
    Task<string> GetPicture64(int timeout = 5000);

    const string DEFAULT_PICTURE_NAME = "folder.jpg";
    #endregion --- Pictures --------------------------------------------

    string ToJson();
    string ToJson(JsonWriterOptions options);

    IMovie ParseJson(string source);
    IMovie ParseJson(JsonElement source);
  }
}
