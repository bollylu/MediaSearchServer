using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearch.Models;

namespace MovieSearchServerServices.MovieService {
  public interface IMovieService {

    string RootPath { get; }

    #region --- Movies --------------------------------------------
    List<string> ExcludedExtensions { get; }

    Task<IMovie> GetMovies(int startPage = 1, int pageSize = 20);
    Task<IMovie> GetMovies(string filter, int startPage = 1, int pageSize = 20);
    Task<IMovies> GetMovies(string group, string filter, int startPage = 1, int pageSize = 20);

    Task<byte[]> GetPicture(string pathname, int timeout = 5000);
    Task<string> GetPicture64(string pathname, int timeout = 5000);
    #endregion --- Movies --------------------------------------------

    #region --- Groups --------------------------------------------
    string CurrentGroup { get; }
    Task<IMovieGroups> GetGroups(string group = "", string filter = "");
    #endregion --- Groups --------------------------------------------



  }
}
