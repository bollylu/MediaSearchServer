using System.Dynamic;

namespace MediaSearch.Models;
public interface IListWithPrincipal<T> : IList<T>, IEnumerable<T>, IToStringIndent {

  /// <summary>
  /// Set the principal value (must exist)
  /// </summary>
  /// <param name="principal">The principal</param>
  /// <returns>The index of the principal value or -1 if no found</returns>
  int SetPrincipal(T principal);

  /// <summary>
  /// Get the principal value
  /// if the list is only one item, that's it
  /// If multiple items, SetPrincipal must have been called or the first items is returned
  /// </summary>
  /// <returns>The principal value</returns>
  T GetPrincipal();

  /// <summary>
  /// Clear the principal
  /// </summary>
  void ClearPrincipal();
}
