namespace MediaSearch.Models;

public interface IMedia : IRecord,
                          ITags,
                          ITitles,
                          IDescriptions,
                          IGroupMembership,
                          IDirty,
                          ICreation,
                          IDisposable,
                          IAsyncDisposable,
                          IPictureContainer,
                          IMultiNames {

  public string Name { get; }

  public EMediaType MediaType { get; set; }

  ///// <summary>
  ///// The filename used to store the media
  ///// </summary>
  //string FileName { get; set; }

  ///// <summary>
  ///// The extension of the filename (e.g.: mkv, pdf, ...)
  ///// </summary>
  //string FileExtension { get; set; }

  ///// <summary>
  ///// The root of the path URI
  ///// </summary>
  //string StorageRoot { get; set; }

  ///// <summary>
  ///// The path of the movie in the storage
  ///// </summary>
  //string StoragePath { get; set; }

  /// <summary>
  /// The date that the movie was added to the library
  /// </summary>
  DateOnly DateAdded { get; set; }

}

