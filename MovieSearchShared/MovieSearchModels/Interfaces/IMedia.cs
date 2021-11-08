using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MovieSearchModels.Interfaces;

namespace MovieSearchModels {
  public interface IMedia : IName, ITags, IMultiNames {

    /// <summary>
    /// The filename used to store the media
    /// </summary>
    string Filename { get; set; }

    /// <summary>
    /// The extension of the filename (e.g.: mkv, pdf, ...)
    /// </summary>
    string FileExtension { get; set; }

    /// <summary>
    /// The root of the path URI
    /// </summary>
    string StorageRoot { get; set; }

    /// <summary>
    /// The path of the movie in the storage
    /// </summary>
    string StoragePath { get; set; }

  }
}
