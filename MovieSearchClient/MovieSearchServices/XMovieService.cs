using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using MovieSearch.Models;

namespace MovieSearch.Services {
  public class XMovieService : IMovieService {
    public string RootPath { get; }
    public List<string> ExcludedExtensions { get; }
    public string CurrentGroup { get; }

    public Task<IMovieGroups> GetGroups(string group = "", string filter = "") {
      IMovieGroups RetVal = new TMovieGroups() { Name = "Fantasy" };
      return Task.FromResult(RetVal);
    }

    public Task<IMovies> GetMovies(string group, string filter, int startPage = 1, int pageSize = 20) {
      IMovies RetVal = new TMovies();
      RetVal.Movies.Add(new TMovie() { LocalName = "Le seigneur des anneaux", Group="Fantasy", LocalPath="Le seigneur des anneaux 1.mvk", Size=8_000_000});
      RetVal.Movies.Add(new TMovie() { LocalName = "Le seigneur des anneaux 2", Group = "Fantasy", LocalPath = "Le seigneur des anneaux 2.mvk", Size = 8_001_000 });
      RetVal.Movies.Add(new TMovie() { LocalName = "Le seigneur des anneaux 3", Group = "Fantasy", LocalPath = "Le seigneur des anneaux 3.mvk", Size = 8_002_000 });
      return Task.FromResult(RetVal);
    }

    public Task<byte[]> GetPicture(string pathname) {
      throw new NotImplementedException();
    }

    public Task<string> GetPicture64(string pathname) {
      throw new NotImplementedException();
    }

    public string GetPictureLocation(string pathname) {
      return "/api/movies/getPicture/missing.jpg";
    }
  }
}
