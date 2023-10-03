namespace MediaSearch.Models;

public interface IMediaSource : IToStringIndent {

  /// <summary>
  /// The date the media source was acquired
  /// </summary>
  DateOnly DateAdded { get; set; }

  /// <summary>
  /// The date the data was created
  /// </summary>
  DateOnly CreationDate { get; set; }

  /// <summary>
  /// The year the data was created
  /// r/o, extracted from CreationDate
  /// if CreationDate is invalid, then 0
  /// </summary>
  int CreationYear { get; }

  /// <summary>
  /// A description of the source
  /// </summary>
  string Description { get; set; }

  /// <summary>
  /// The language(s) associated with the source
  /// If more than one, one principal must be designated
  /// </summary>
  IListWithPrincipal<ELanguage> Languages { get; set; }

  IMediaSourceProperties Properties { get; }
}


