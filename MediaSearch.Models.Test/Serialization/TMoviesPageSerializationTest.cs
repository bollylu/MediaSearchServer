namespace MediaSearch.Models.Serialization.Test;

[TestClass]
public class TMoviesPageSerializationTest {

  internal TMSTableJsonMovie Database => _Database ??= new TMSTableJsonMovie("data", "movies");
  private TMSTableJsonMovie? _Database;

  [TestMethod]
  public void Serialize() {

    Assert.IsTrue(Database.Exists());
    Assert.IsTrue(Database.OpenOrCreate());
    Assert.IsTrue(Database.Load());

    IList<IMovie> Movies = Database.GetAll(2).Cast<IMovie>().ToList();

    IMoviesPage Source = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    Source.Movies.AddRange(Movies);

    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);

    string Target = Source.ToJson();

    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Deserialize() {
    Assert.IsTrue(Database.Exists());
    Assert.IsTrue(Database.OpenOrCreate());
    Assert.IsTrue(Database.Load());
    IList<IMovie> Movies = Database.GetAll(2).Cast<IMovie>().ToList();

    IMoviesPage MoviesPage = new TMoviesPage() {
      Name = "Sélection",
      Page = 1,
      AvailablePages = 3,
      AvailableMovies = 3456,
      Source = "Andromeda"
    };
    MoviesPage.Movies.AddRange(Movies);

    string Source = MoviesPage.ToJson();
    TraceMessage($"{nameof(Source)} : {Source.GetType().Name}", Source);

    IMoviesPage? Target = IJson<TMoviesPage>.FromJson(Source);
    Assert.IsNotNull(Target);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

}

