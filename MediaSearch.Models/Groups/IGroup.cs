namespace MediaSearch.Models;
public interface IGroup {
  /// <summary>
  /// The name of the group
  /// </summary>
  string Name { get; set; }

  /// <summary>
  /// The inner groups of this group
  /// </summary>
  IList<IGroup> Groups { get; }

  /// <summary>
  /// The medias belonging to this group
  /// </summary>
  IList<IMedia> Medias { get; }
}
