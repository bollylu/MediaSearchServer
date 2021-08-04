using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearch.Models;

namespace MovieSearch.Services {
  public interface IMovieService {

    string RootPath { get; }

    #region --- Movies --------------------------------------------
    List<string> ExcludedExtensions { get; }
    Task<IMovies> GetMovies(string group, string filter, int startPage = 0, int pageSize = 20);
    Task<byte[]> GetPicture(string pathname);
    Task<string> GetPicture64(string pathname);
    #endregion --- Movies --------------------------------------------

    #region --- Groups --------------------------------------------
    string CurrentGroup { get; }
    Task<IMovieGroups> GetGroups(string group = "", string filter = "");
    #endregion --- Groups --------------------------------------------

    string GetPictureLocation(string pathname);

  }
}
