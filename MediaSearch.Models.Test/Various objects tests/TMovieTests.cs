namespace MediaSearch.Models.Test;

[TestClass]
public class TMovieTests {

  [TestMethod]
  public void Instanciate_TMovie() {
    Message("Instanciate TMovie");
    TMovie Target = new TMovie() { Name = "Test Movie" };
    Assert.IsNotNull(Target);
    Dump(Target);

    Assert.IsTrue(Target.MediaSources.IsEmpty());
    Assert.IsTrue(Target.Tags.IsEmpty());
    Assert.IsTrue(Target.AltNames.IsEmpty());
    Assert.IsTrue(Target.Descriptions.IsEmpty());
    Assert.IsFalse(Target.IsDirty);
    Assert.AreEqual(EMediaType.Unknown, Target.MediaType);

    Message("Default values are ok");
    Ok();
  }

  [TestMethod]
  public void Add_Source_To_TMovie() {
    Message("Instanciate TMovie");
    TMovie Target = new TMovie() { Name = "Test Movie" };
    Dump(Target);

    Message("Instanciate TMediaSourceMovie");
    TMediaSourceMovie Source = new TMediaSourceMovie() {
      FileName = "test movie",
      StorageRoot = @"\\server\sharename",
      StoragePath = @"movies\Action",
      FileExtension = ".mkv",
      Language = ELanguage.English,
      Size = 123456
    };
    Dump(Source);

    Message("Add source to movie");
    Target.MediaSources.Add(Source);

    Assert.AreEqual(1, Target.MediaSources.Count);
    Dump(Target);

    Ok();

  }
}
