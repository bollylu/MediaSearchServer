namespace MediaSearch.Models;
public interface IPicture {

  /// <summary>
  /// The name of the picture
  /// </summary>
  string Name { get; init; }

  /// <summary>
  /// The bytes of the picture
  /// </summary>
  byte[] Data { get; set; }

  /// <summary>
  /// The kind of picture
  /// </summary>
  EPictureType PictureType { get; init; }

  /// <summary>
  /// Language associated with picture
  /// </summary>
  ELanguage Language { get; init; }

  /// <summary>
  /// Load the picture in memory from a physical location
  /// </summary>
  /// <param name="location">The url where to find the picture</param>
  /// <param name="withResize">Indicate that the result must be resized to the DEFAULT values of width and height</param>
  /// <returns><see langword="true"/> if successful, <see langword="false"/> otherwise</returns>
  Task<bool> LoadAsync(string location, bool withResize);

  /// <summary>
  /// Resize the picture to required dimensions
  /// </summary>
  /// <param name="width">The width</param>
  /// <param name="height">The height</param>
  /// <returns>The bytes of the new picture, resized</returns>
  IPicture? Resize(int width, int height);
}
