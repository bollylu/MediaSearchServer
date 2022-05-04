namespace MediaSearch.Models;

public interface IMSTableHeader<T> where T : IMedia {

  /// <summary>
  /// The name of the table
  /// </summary>
  public string Name { get; set; }

  /// <summary>
  /// A description of the table content
  /// </summary>
  public string Description { get; set; }

  /// <summary>
  /// When is the last update to any item in this table
  /// </summary>
  public DateTime LastUpdate { get; set; }

  /// <summary>
  /// Table can contain only only data type
  /// </summary>
  public Type DbType { get; }

  /// <summary>
  /// The data is originating from a media source, where we can also retrieve items like pictures, ...
  /// </summary>
  public IMediaSource<T> MediaSource { get; set; }

  /// <summary>
  /// Transforms the table to a string, with indentation
  /// </summary>
  /// <param name="indent">The current indentation</param>
  /// <returns>A string representing the table</returns>
  string ToString(int indent);

  /// <summary>
  /// The index of the records of the table, Name <=> Id
  /// </summary>
  IMSIndex<T, string> IndexByName { get; }

}
