using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieSearch.Models {
  public interface IMovie {

    /// <summary>
    /// The name of the movie in local storage
    /// </summary>
    string LocalName { get; }

    /// <summary>
    /// A list of alternate names (0 <= count <= n)
    /// </summary>
    List<string> AltNames { get; }

    /// <summary>
    /// The path of the movie in the storage
    /// </summary>
    string LocalPath { get; }

    /// <summary>
    /// The group where the movie belongs
    /// Group components are separated by '/' (eg. Comédie/Aventure)
    /// </summary>
    string Group { get; }

    /// <summary>
    /// The movie size
    /// </summary>
    long Size { get; }

    /// <summary>
    /// The movie cover
    /// </summary>
    string Picture { get; }

    /// <summary>
    /// A list of tags (0 <= count <= n)
    /// </summary>
    List<string> Tags { get; }
  }
}
