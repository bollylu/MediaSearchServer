namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMoviesPageSerializationTest {

  internal IMSDatabase Database => _Database ??= new TMSDatabaseJson("data", "demo");
  private IMSDatabase? _Database;

  [TestMethod]
  public void Serialize() {

    Assert.IsTrue(Database.Exists());
    Assert.IsTrue(Database.Open());

    IMSTable<IMovie>? MovieTable = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(MovieTable);

    IList<IMovie> Movies = MovieTable.GetAll(2).Cast<IMovie>().ToList();

    IMoviesPage Source = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    Source.Movies.AddRange(Movies);

    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Deserialize() {
    Assert.IsTrue(Database.Exists());
    Assert.IsTrue(Database.Open());

    IMSTable<IMovie>? MovieTable = Database.GetTable("movies") as IMSTable<IMovie>;
    Assert.IsNotNull(MovieTable);

    IList<IMovie> Movies = MovieTable.GetAll(2).Cast<IMovie>().ToList();

    IMoviesPage MoviesPage = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    MoviesPage.Movies.AddRange(Movies);

    string Source = MoviesPage.ToJson();
    TraceBox($"{nameof(Source)} : {Source.GetType().Name}", Source);

    IMoviesPage? Target = IJson<TMoviesPage>.FromJson(Source);
    Assert.IsNotNull(Target);
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

}

