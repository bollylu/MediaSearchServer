using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MovieSearch.Models;

namespace MovieSearchTest {

  [TestClass]
  public class TMovieSerializationTest {

    [TestMethod]
    public void SerializeTMovie() {
      TMovie Movie = new TMovie() {
        LocalName = "Un bon film",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder",
        Size = 123456789,
        Group = "GoodMovies",
        OutputYear = 1966,
      };
      Movie.AltNames.Add("A good movie");

      string JsonMovie = Movie.ToJson();

      Assert.IsNotNull(JsonMovie);
      Console.WriteLine(JsonMovie);
    }

    [TestMethod]
    public void DeserializeTMovie() {
      TMovie Movie = new TMovie() {
        LocalName = "Un bon film",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder",
        Size = 123456789,
        Group = "GoodMovies",
        OutputYear = 1966,
      };
      Movie.AltNames.Add("A good movie");
      Movie.AltNames.Add("A nice film");
      Movie.Tags.Add("Comédie");
      Movie.Tags.Add("Romantique");
      Movie.Tags.Add("Science-fiction");

      string JsonMovie = Movie.ToJson();

      IMovie Target = TMovie.FromJson(JsonMovie);

      Assert.IsNotNull(Target);
      Assert.AreEqual(123456789,Target.Size);
      Console.WriteLine(Target.ToString());
    }

    [TestMethod]
    public void SerializeTMovies() {

      IMovie Movie1 = new TMovie() {
        LocalName = "Un bon film 1",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder",
        Size = 123456789,
        Group = "GoodMovies",
        OutputYear = 1966
      };
      Movie1.AltNames.Add("A good movie");

      IMovie Movie2 = new TMovie() {
        LocalName = "Un bon film 2",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder2",
        Size = 987654321,
        Group = "GoodMovies",
        OutputYear = 1967
      };
      Movie2.AltNames.Add("A good movie - Return");
      Movie2.AltNames.Add("A nice film 2");
      Movie2.Tags.Add("Comédie");
      Movie2.Tags.Add("Romantique");
      Movie2.Tags.Add("Science-fiction");

      IMovies Movies = new TMovies() {
        Name = "Sélection",
        Page = 1,
        AvailablePages = 3,
        Source = "Andromeda"
      };
      Movies.Movies.Add(Movie1);
      Movies.Movies.Add(Movie2);

      string JsonMovies = Movies.ToJson(new JsonWriterOptions() { Indented = true });

      Assert.IsNotNull(JsonMovies);
      Console.WriteLine(JsonMovies);
    }

    [TestMethod]
    public void DeserializeTMovies() {
      #region --- Data filling --------------------------------------------
      IMovie Movie1 = new TMovie() {
        LocalName = "Un bon film 1",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder",
        Size = 123456789,
        Group = "GoodMovies",
        OutputYear = 1966
      };
      Movie1.AltNames.Add("A good movie");

      IMovie Movie2 = new TMovie() {
        LocalName = "Un bon film 2",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder2",
        Size = 987654321,
        Group = "GoodMovies",
        OutputYear = 1967
      };
      Movie2.AltNames.Add("A good movie - Return");
      Movie2.AltNames.Add("A nice film 2");
      Movie2.Tags.Add("Comédie");
      Movie2.Tags.Add("Romantique");
      Movie2.Tags.Add("Science-fiction");

      IMovies Movies = new TMovies() {
        Name = "Sélection",
        Page = 1,
        AvailablePages = 3,
        Source = "Andromeda"
      };
      Movies.Movies.Add(Movie1);
      Movies.Movies.Add(Movie2);
      #endregion --- Data filling --------------------------------------------

      string JsonMovies = Movies.ToJson(new JsonWriterOptions() { Indented = true });
      Assert.IsNotNull(JsonMovies);
      Console.WriteLine(JsonMovies);

      IMovies Target = TMovies.FromJson(JsonMovies);

      Console.WriteLine(Target.ToString());
    }

    [TestMethod]
    public void DeserializeTMovies_Arrays_Empty() {

      IMovie Movie1 = new TMovie() {
        LocalName = "Un bon film 1",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder",
        Size = 123456789,
        Group = "GoodMovies",
        OutputYear = 1966
      };
      #region --- Data filling --------------------------------------------

      IMovie Movie2 = new TMovie() {
        LocalName = "Un bon film 2",
        Storage = "\\\\Andromeda\\films",
        LocalPath = "Folder\\subfolder2",
        Size = 987654321,
        Group = "GoodMovies",
        OutputYear = 1967
      };

      IMovies Movies = new TMovies() {
        Name = "Sélection",
        Page = 1,
        AvailablePages = 3,
        Source = "Andromeda"
      };
      Movies.Movies.Add(Movie1);
      Movies.Movies.Add(Movie2);
      #endregion --- Data filling --------------------------------------------

      string JsonMovies = Movies.ToJson();

      Assert.IsNotNull(JsonMovies);
      Console.WriteLine(JsonMovies);

      IMovies Target = TMovies.FromJson(JsonMovies);
      Console.WriteLine(Target.ToString());
    }

  }
}
