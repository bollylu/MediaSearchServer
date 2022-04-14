namespace MediaSearch.Models.Test;

[TestClass]
public class TMediaSourceMovieTests {

  [TestMethod]
  public void InstanciateTMediaSourceMovie() {
    TMediaSourceMovie Target = new TMediaSourceMovie() {
      Name = "test file",
      Description = "data for test purpose",
      Database = new TMediaSearchDatabaseJson("data", "movies")
    };
    Assert.IsNotNull(Target);
    Assert.IsNotNull(Target.Database);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  //[TestMethod]
  //public async Task LoadDataFromProvider() {
  //  TMediaSourceMovie Target = new TMediaSourceMovie() {
  //    Name = "test file",
  //    Description = "data for test purpose",
  //    Database = new TMediaSearchDatabaseJson("data", "movies.json")
  //  };
  //  Assert.IsNotNull(Target);
  //  Assert.IsNotNull(Target.Database);

  //  ((IMediaSearchDatabasePersistent)Target.Database).Open();
  //  await ((IMediaSearchDatabasePersistent)Target.Database).LoadAsync(CancellationToken.None);

  //  Assert.IsTrue(Target.Database.Any());
  //  TraceMessage("Movies count", Target.Database.Count());
  //  TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  //}
}