namespace MediaSearch.Models;
public interface IMediaInfoCreation {

  /// <summary>
  /// The date when the media was created
  /// </summary>
  DateOnly CreationDate { get; set; }

  /// <summary>
  /// The year when the media was created
  /// </summary>
  int CreationYear { get; }

}
