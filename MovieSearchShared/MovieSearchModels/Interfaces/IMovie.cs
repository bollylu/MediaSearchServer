﻿namespace MovieSearchModels;

public interface IMovie : IMedia{

  /// <summary>
  /// The normalized extension of the movie (container type) based on file extension
  /// </summary>
  EMovieExtension Extension { get; }

  /// <summary>
  /// The group where the movie belongs
  /// Group components are separated by '/' (eg. Comédie/Aventure)
  /// </summary>
  string Group { get; set; }

  /// <summary>
  /// The movie size
  /// </summary>
  long Size { get; set; }

  /// <summary>
  /// The year when the movie was distributed on cinema/DTV/...
  /// </summary>
  int OutputYear { get; set; }

  #region --- Pictures --------------------------------------------
  ///// <summary>
  ///// Get the movie cover in bytes
  ///// </summary>
  //Task<byte[]> GetPicture(int timeout = 5000);

  ///// <summary>
  ///// Get the movie cover in base64
  ///// </summary>
  //Task<string> GetPicture64(int timeout = 5000);

  const string DEFAULT_PICTURE_NAME = "folder.jpg";
  #endregion --- Pictures --------------------------------------------

  string ToJson();
  string ToJson(JsonWriterOptions options);

  IMovie ParseJson(string source);
  IMovie ParseJson(JsonElement source);
}
