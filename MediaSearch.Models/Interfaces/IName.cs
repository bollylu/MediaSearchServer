namespace MediaSearch.Models;

public interface IName {

  /// <summary>
  /// The unique name of the item
  /// </summary>
  string Name { get; }

  /// <summary>
  /// An extended description
  /// </summary>
  string Description { get; }
  
}

