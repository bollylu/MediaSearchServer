namespace MediaSearch.Models.MediaSearchDatabase.Test;

[TestClass]
public class TMediaSearchDatabaseJsonTests {


  [TestMethod]
  public void Instanciate_Empty_TMediaSearchDatabaseJson() {

    IMediaSearchDatabase Target = new TMediaSearchDatabaseJson(Path.GetTempPath(), "empty");
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Empty_TMediaSearchDatabaseJson_OpenThenAddItems_ThenCleanup() {
    IMediaSearchDatabase Target = new TMediaSearchDatabaseJson(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");

    Target.Open();
    Assert.IsTrue(Target.Exists());

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add("A good movie");
    Target.Add(Media);

    TraceMessage("Target after one addition", Target.ToString());
    Assert.AreEqual(1, Target.GetAll().Count());

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321
    };
    Media2.Descriptions.Add("Bad movies");
    Target.Add(Media2);

    TraceMessage("Target after two additions", Target.ToString());

    Assert.AreEqual(2, Target.GetAll().Count());

    Target.Remove();

    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMediaSearchDatabaseJson_AddItems_ThenSave() {
    IMediaSearchDatabase Target = new TMediaSearchDatabaseJson(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");

    Target.Open();
    Assert.IsTrue(Target.Exists());

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add("A good movie");
    Target.Add(Media);

    TraceMessage("Target after one addition", Target.ToString());
    Assert.AreEqual(1, Target.GetAll().Count());

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321
    };
    Media2.Descriptions.Add("Bad movies");
    Target.Add(Media2);
    Assert.IsTrue(Target.IsDirty);

    TraceMessage("Target after two additions", Target);

    await Target.SaveAsync(CancellationToken.None);
    Assert.IsFalse(Target.IsDirty);

    foreach (string RecordItem in Target.GetAll().Select(r => r.Id)) {
      string RealFullFilename = Path.Combine(((TMediaSearchDatabaseJson)Target).DatabaseFullName, RecordItem);
      TraceMessage($"RawContent of {RecordItem}", await File.ReadAllTextAsync($"{RealFullFilename}.json"));
    }

    await Target.CloseAsync(CancellationToken.None);
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMediaSearchDatabaseJson_AddItems_WithAutoSave_Async() {
    IMediaSearchDatabase Target = new TMediaSearchDatabaseJson(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");
    Target.AutoSave = true;

    Target.Open();
    Assert.IsTrue(Target.Exists());
    Assert.IsFalse(Target.IsDirty);

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add("A good movie");

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321
    };
    Media2.Descriptions.Add("Bad movies");

    await Target.AddAsync(Media, CancellationToken.None);
    Assert.IsFalse(Target.IsDirty);

    TraceMessage("Db after 1 addition", Directory.EnumerateFiles(((TMediaSearchDatabaseJson)Target).DatabaseFullName).Count());

    await Target.AddAsync(Media2, CancellationToken.None);
    Assert.IsFalse(Target.IsDirty);

    TraceMessage("Db after 2 additions", Directory.EnumerateFiles(((TMediaSearchDatabaseJson)Target).DatabaseFullName).Count());

    TraceMessage("Target after two additions", Target);

    await Target.CloseAsync(CancellationToken.None);

    foreach (string RecordItem in Target.GetAll().Select(r => r.Id)) {
      string RealFullFilename = Path.Combine(((TMediaSearchDatabaseJson)Target).DatabaseFullName, RecordItem);
      TraceMessage($"RawContent of {RecordItem}", await File.ReadAllTextAsync($"{RealFullFilename}.json"));
    }
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void Duplicate_TMediaSearchDatabaseJson() {
    IMediaSearchDatabase Source = new TMediaSearchDatabaseJson(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");

    Assert.IsTrue(Source.Open());
    Assert.IsTrue(Source.Exists());

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add("A good movie");

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321
    };
    Media2.Descriptions.Add("Bad movies");

    Source.Add(Media);
    Assert.IsTrue(Source.IsDirty);

    Source.Add(Media2);
    Assert.IsTrue(Source.IsDirty);

    TraceMessage($"{nameof(Source)} after 2 additions : {Source.GetType().Name}", Source);

    Assert.IsTrue(Source.Save());
    Assert.IsFalse(Source.IsDirty);
    Source.Close();

    IMediaSearchDatabase Target = new TMediaSearchDatabaseJson(Source) {
      DatabaseName = $"TestAdd{Random.Shared.Next()}"
    };
    Assert.IsFalse(Target.Exists());
    Assert.IsTrue(Target.Open());
    Assert.IsTrue(Target.Save());
    Assert.IsTrue(Target.Exists());
    Target.Close();

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Assert.AreEqual(2, Target.GetAll().Count());

    Source.Remove();
    Target.Remove();
    Assert.IsFalse(Source.Exists());
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void Instance_TMediaSearchDatabaseJson_Load() {
    IMediaSearchDatabase Target = new TMediaSearchDatabaseJson("data", "movies");
    Assert.IsTrue(Target.Exists());

    Assert.IsTrue(Target.Open());
    Assert.IsTrue(Target.Load());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Target.Close();
  }
}