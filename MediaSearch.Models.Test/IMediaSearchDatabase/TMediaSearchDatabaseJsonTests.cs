namespace MediaSearch.Models.MediaSearchDatabase.Test;

[TestClass]
public class TMediaSearchDatabaseJsonTests {


  [TestMethod]
  public void Instanciate_Empty_TMediaSearchDatabaseJson() {

    string Name = Random.Shared.Next().ToString();
    IMediaSource Source = new TMediaSource(EMediaSourceType.Unknown, Name);

    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), Name, Source);
    Assert.IsNotNull(Target);
    Assert.AreEqual(Name, Target.Header.Name);
    Assert.AreEqual(EMediaSourceType.Unknown, Target.Header.DbType);
    Assert.IsTrue(Target.IsEmpty());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_Empty_TMediaSearchDatabaseJson_Movies() {

    string Name = Random.Shared.Next().ToString();
    IMediaSource Source = new TMediaSource(EMediaSourceType.Movie, Name);

    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), Name, Source);
    Assert.IsNotNull(Target);
    Assert.AreEqual(Name, Target.Header.Name);
    Assert.AreEqual(EMediaSourceType.Movie, Target.Header.DbType);
    Assert.IsTrue(Target.IsEmpty());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_Empty_TMediaSearchDatabaseJson_DoesNotExist() {

    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), "empty");
    Assert.IsFalse(Target.Exists());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void Instanciate_TMediaSearchDatabaseJson_DoesNotExist_Create_Remove() {
    IMediaSource Source = new TMediaSource(EMediaSourceType.Movie, "");
    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), Random.Shared.Next().ToString(), Source);
    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Assert.IsFalse(Target.Exists());

    Assert.IsTrue(Target.OpenOrCreate());
    Assert.IsTrue(Target.Exists());

    Assert.AreEqual(1, Directory.EnumerateFiles(((TMSTableJsonMovie)Target).DatabaseFullName, "*").Count());
    TraceMessage("Header", File.ReadAllText(((TMSTableJsonMovie)Target).HeaderFullFilename));

    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void Empty_TMediaSearchDatabaseJson_OpenThenAddItems_ThenCleanup() {
    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");

    Target.OpenOrCreate();
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

    Assert.AreEqual(2, Target.Count());

    Target.Remove();

    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void TMediaSearchDatabaseJson_AddItems_ThenSave() {
    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), Random.Shared.Next().ToString());

    Target.OpenOrCreate();
    Assert.IsTrue(Target.Exists());

    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add("A good movie");
    Target.Add(Media);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name} : after 1 addition", Target);
    Assert.AreEqual(1, Target.Count());

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321
    };
    Media2.Descriptions.Add("Bad movies");
    Target.Add(Media2);
    Assert.IsTrue(Target.IsDirty);

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name} : after 2 additions", Target);

    Target.Save();
    Assert.IsFalse(Target.IsDirty);

    foreach (string RecordItem in Target.GetAll().Select(r => r.Id)) {
      string RealFullFilename = Path.Combine(((TMSTableJsonMovie)Target).DatabaseFullName, RecordItem);
      TraceMessage($"RawContent of {RecordItem}", File.ReadAllText($"{RealFullFilename}.record.json"));
    }

    Target.Close();
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMediaSearchDatabaseJson_AddItems_ThenSaveAsync() {
    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");

    Target.OpenOrCreate();
    Assert.IsTrue(Target.Exists());

    #region --- Medias --------------------------------------------
    IMedia Media = new TMovie("test movie", 1966) {
      Size = 1234
    };
    Media.Descriptions.Add("A good movie");

    IMedia Media2 = new TMovie("other movie", 1989) {
      Size = 4321
    };
    Media2.Descriptions.Add("Bad movies");
    #endregion --- Medias --------------------------------------------

    Assert.IsFalse(Target.IsDirty);

    Target.Add(Media);
    Assert.IsTrue(Target.IsDirty);
    TraceMessage("Target after one addition", Target.ToString());
    Assert.AreEqual(1, Target.Count());
    
    Target.Add(Media2);
    Assert.IsTrue(Target.IsDirty);
    TraceMessage("Target after two additions", Target);
    Assert.AreEqual(2, Target.Count());

    Assert.IsTrue(await Target.SaveAsync());
    Assert.IsFalse(Target.IsDirty);

    foreach (string RecordItem in Target.GetAll().Select(r => r.Id)) {
      string RealFullFilename = Path.Combine(((TMSTableJsonMovie)Target).DatabaseFullName, RecordItem);
      TraceMessage($"RawContent of {RecordItem}", File.ReadAllText($"{RealFullFilename}.record.json"));
    }

    Target.Close();
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMediaSearchDatabaseJson_AddItems_WithAutoSave_Async() {
    IMediaSearchDataTable Target = new TMSTableJsonMovie(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");
    Target.AutoSave = true;

    Target.OpenOrCreate();
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

    TraceMessage("Db after 1 addition", Directory.EnumerateFiles(((TMSTableJsonMovie)Target).DatabaseFullName).Count());

    await Target.AddAsync(Media2, CancellationToken.None);
    Assert.IsFalse(Target.IsDirty);

    TraceMessage("Db after 2 additions", Directory.EnumerateFiles(((TMSTableJsonMovie)Target).DatabaseFullName).Count());

    TraceMessage("Target after two additions", Target);

    await Target.CloseAsync(CancellationToken.None);

    foreach (string RecordItem in Target.GetAll().Select(r => r.Id)) {
      string RealFullFilename = Path.Combine(((TMSTableJsonMovie)Target).DatabaseFullName, RecordItem);
      TraceMessage($"RawContent of {RecordItem}", await File.ReadAllTextAsync($"{RealFullFilename}.json"));
    }
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void Duplicate_TMediaSearchDatabaseJson() {
    IMediaSearchDataTable Source = new TMSTableJsonMovie(Path.GetTempPath(), $"TestAdd{Random.Shared.Next()}");

    Assert.IsTrue(Source.OpenOrCreate());
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

    IMediaSearchDataTable Target = new TMediaSearchMovieDatabaseJson(Source) {
      DatabaseName = $"TestAdd{Random.Shared.Next()}"
    };
    Assert.IsFalse(Target.Exists());
    Assert.IsTrue(Target.OpenOrCreate());
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
    IMediaSearchDataTable Target = new TMSTableJsonMovie("data", "movies");
    Assert.IsTrue(Target.Exists());

    Assert.IsTrue(Target.OpenOrCreate());
    Assert.IsTrue(Target.Load());

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Target.Close();
  }

  [TestMethod]
  public async Task Instance_TMediaSearchDatabaseJson_LoadAsync() {
    IMediaSearchDataTable Target = new TMSTableJsonMovie("data", "movies");
    Assert.IsTrue(Target.Exists());

    Assert.IsTrue(Target.OpenOrCreate());
    Assert.IsTrue(await Target.LoadAsync(CancellationToken.None));

    TraceMessage($"{nameof(Target)} : {Target.GetType().Name}", Target);

    Target.Close();
  }
}