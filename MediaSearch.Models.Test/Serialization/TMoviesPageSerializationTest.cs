namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMoviesPageSerializationTest {

  internal IMSDatabase Database => _Database ??= new TMSDatabaseJson("data", "demo");
  private IMSDatabase? _Database;

  [TestMethod]
  public void Serialize() {

    IList<IMovie> Movies = new List<IMovie> {
      new TMovie("Le chat noir", 1960),
      new TMovie("Le chat blanc", 1962)
    };

    IMoviesPage Source = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    Source.Movies.AddRange(Movies);

    Dump(Source);

    string Target = IJson.ToJson(Source);

    Assert.IsNotNull(Target);
    Dump(Target);

    Ok();
  }

  [TestMethod]
  public void Deserialize() {

    IList<IMovie> Movies = new List<IMovie> {
      new TMovie("Le chat noir", 1960),
      new TMovie("Le chat blanc", 1962)
    };

    IMoviesPage MoviesPage = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    MoviesPage.Movies.AddRange(Movies);

    string Source = IJson.ToJson(MoviesPage);
    Dump(Source);

    IMoviesPage? Target = IJson.FromJson<TMoviesPage>(Source);
    Assert.IsNotNull(Target);
    Dump(Target);
    Ok();
  }

}

