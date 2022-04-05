namespace MediaSearch.Models;

public interface IMedia : IName, ITags, ITitles {

  /// <summary>
  /// Identifier for this item
  /// </summary>
  string Id { get; }

  #region --- Physical storage --------------------------------------------
  /// <summary>
  /// The filename used to store the media
  /// </summary>
  string FileName { get; set; }

  /// <summary>
  /// The extension of the filename (e.g.: mkv, pdf, ...)
  /// </summary>
  string FileExtension { get; set; }

  /// <summary>
  /// The root of the path URI
  /// </summary>
  string StorageRoot { get; set; }

  /// <summary>
  /// The path of the movie in the storage
  /// </summary>
  string StoragePath { get; set; }

  /// <summary>
  /// The date that the movie was added to the library
  /// </summary>
  DateOnly DateAdded { get; set; } 
  #endregion --- Physical storage --------------------------------------------

  /// <summary>
  /// The media size
  /// </summary>
  long Size { get; set; }

  /// <summary>
  /// The group where the movie belongs
  /// </summary>
  string Group { get; set; }

  /// <summary>
  /// Indicate if the movie belongs to a group
  /// </summary>
  bool IsGroupMember { get; }

  /// <summary>
  /// The year when the media was created
  /// </summary>
  int CreationYear { get; set; }
}

