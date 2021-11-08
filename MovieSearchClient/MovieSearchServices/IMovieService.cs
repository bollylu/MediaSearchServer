using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MovieSearchModels;

namespace MovieSearchClientServices {
  public interface IMovieService {

    string RootPath { get; }

    #region --- Movies --------------------------------------------
    List<string> ExcludedExtensions { get; }
    Task<IMovies> GetMovies(string filter, int startPage = 1, int pageSize = 20);
    Task<byte[]> GetPicture(string pathname, int w = 128, int h = 160);
    Task<string> GetPicture64(IMovie movie);
    #endregion --- Movies --------------------------------------------

    #region --- Groups --------------------------------------------
    string CurrentGroup { get; }
    Task<IMovieGroups> GetGroups(string group = "", string filter = "");
    #endregion --- Groups --------------------------------------------

    string GetPictureLocation(string pathname);

  }
}
