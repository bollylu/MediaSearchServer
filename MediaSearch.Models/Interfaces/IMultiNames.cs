﻿namespace MediaSearch.Models;

public interface IMultiNames {

  /// <summary>
  /// A list of alternate KeyValuePairs (language & name) (0 <= count <= n)
  /// </summary>
  Dictionary<string, string> AltNames { get; }

}

