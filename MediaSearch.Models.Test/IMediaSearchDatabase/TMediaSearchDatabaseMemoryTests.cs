namespace MediaSearch.Models.MediaSearchDatabase.Test;

[TestClass]
public class TMediaSearchDatabaseMemoryTests {

  [TestMethod]
  public void Instanciate_TMediaSearchDatabaseMemory_Empty() {
    IMediaSearchDataTable Target = new TMediaSearchMovieDatabaseMemory();
    Assert.IsNotNull(Target);

    Assert.IsNotNull(Target.Header);
    Target.Header.Name = "test";
    Target.Header.Description = "test db";

    Assert.IsTrue(Target.IsEmpty());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Empty_TMediaSearchDatabaseMemory_AddItems() {
    IMediaSearchDataTable Target = new TMediaSearchMovieDatabaseMemory();
    Target.Header.Name = "test";
    Target.Header.Description = "test db";
    Assert.IsNotNull(Target);

    Assert.IsTrue(Target.IsEmpty());

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add(ELanguage.English, "Good movie");
    Target.Add(Media);

    TraceMessage($"{nameof(Target)} after 1 addition : {Target.GetType().Name}", Target);
    Assert.AreEqual(1, Target.GetAll().Count());

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321,
    };
    Media2.Descriptions.Add(ELanguage.English, "Bad movie");
    Target.Add(Media2);

    TraceMessage($"{nameof(Target)} after 2 additions : {Target.GetType().Name}", Target);

    Assert.AreEqual(2, Target.GetAll().Count());
  }

  [TestMethod]
  public void Duplicate_TMediaSearchDatabaseMemory() {
    IMediaSearchDataTable Source = new TMediaSearchMovieDatabaseMemory();
    Source.Header.Name = "test";
    Source.Header.Description = "test db";

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add(ELanguage.English, "Good movie");
    Source.Add(Media);

    TraceMessage($"{nameof(Source)} after 1 addition : {Source.GetType().Name}", Source);
    Assert.AreEqual(1, Source.GetAll().Count());

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321,
    };
    Media2.Descriptions.Add(ELanguage.English, "Bad movie");
    Source.Add(Media2);

    TraceMessage($"{nameof(Source)} after 2 additions : {Source.GetType().Name}", Source);

    IMediaSearchDataTable Target = new TMediaSearchMovieDatabaseMemory(Source);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.AreEqual(2, Target.GetAll().Count());
  }
}