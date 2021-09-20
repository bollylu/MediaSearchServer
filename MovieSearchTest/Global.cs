using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearchServerServices.MovieService;

namespace MovieSearchTest {
  [TestClass]
  public static class Global {

    public static IMovieService MovieService;
    public static IMovieCache MovieCache;

    public const string STORAGE = @"\\andromeda.sharenet.priv\films";

  }
}
