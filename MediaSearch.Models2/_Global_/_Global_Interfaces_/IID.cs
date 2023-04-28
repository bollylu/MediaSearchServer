namespace MediaSearch.Models;
public interface IId<T> {

  /// <summary>
  /// String identifier
  /// </summary>
  T Id { get; }

}
