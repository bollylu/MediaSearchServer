using System.Collections.Concurrent;
using System.Globalization;
using MediaSearch.Models2.Support.Filter;

namespace MediaSearch.Models;
public interface IMultiItemsSelection {

  /// <summary>
  /// List of items
  /// </summary>
  ConcurrentBag<string> Items { get; }

  /// <summary>
  /// How to combine multiple items
  /// </summary>
  EFilterType Selection { get; }

  /// <summary>
  /// Are there any item in Items ?
  /// </summary>
  /// <returns></returns>
  bool Any() => Items.Any();

  /// <summary>
  /// Is Items empty ?
  /// </summary>
  /// <returns></returns>
  bool IsEmpty() => !Any();

  /// <summary>
  /// Check if some or all item(s) in the list match another list of items
  /// </summary>
  /// <param name="target">The target list of items to match against</param>
  /// <param name="options">How to compare</param>
  /// <returns><see langword="true"/> if the comparison matches, <see langword="false"/> otherwise</returns>
  bool IsMatch(IEnumerable<string> target, CompareOptions options = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

  /// <summary>
  /// Check if some or all item(s) in the list match the string content
  /// </summary>
  /// <param name="target">The string to match against</param>
  /// <param name="options">How to compare</param>
  /// <returns><see langword="true"/> if the comparison matches, <see langword="false"/> otherwise</returns>
  bool IsMatch(string target, CompareOptions options = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace);

  /// <summary>
  /// Clear the items and reset Selection to Any
  /// </summary>
  void Clear();
}
