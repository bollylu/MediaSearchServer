namespace MovieSearchModels;

public interface IName {

  /// <summary>
  /// The unique name of the item
  /// </summary>
  string Name { get; set; }

  /// <summary>
  /// An extended description
  /// </summary>
  string Description { get; set; }

}

