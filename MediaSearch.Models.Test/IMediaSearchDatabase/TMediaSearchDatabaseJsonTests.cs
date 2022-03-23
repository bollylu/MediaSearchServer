namespace MediaSearch.Models.MediaSearchDatabase.Test;

[TestClass]
public class TMediaSearchDatabaseJsonTests {


  [TestMethod]
  public void Instanciate_Empty_TMediaSearchDatabaseJson() {
    TMediaSearchDatabaseJson Target = new TMediaSearchDatabaseJson() {
      Name = "test",
      Description = "test db",
      StoragePath = Path.GetTempPath(),
      StorageFilename = "empty.json"
    };
    Assert.IsNotNull(Target);
    Assert.IsTrue(Target.IsEmpty());

    TraceMessage("Target", Target.ToString());
  }

  [TestMethod]
  public void Empty_TMediaSearchDatabaseJson_OpenThenAddItems_ThenCleanup() {
    TMediaSearchDatabaseJson Target = new TMediaSearchDatabaseJson() {
      Name = "test",
      Description = "test db",
      StoragePath = Path.GetTempPath(),
      StorageFilename = $"TestAdd{Random.Shared.Next()}.json"
    };

    Target.Open();
    Assert.IsTrue(Target.Exists());

    IMedia Media = new TMovie() {
      Name = "test movie",
      Description = "Good movie",
      Size = 1234,
      CreationYear = 1966
    };
    Target.Add(Media);

    TraceMessage("Target after one addition", Target.ToString());
    Assert.AreEqual(1, Target.GetAll().Count());

    IMedia Media2 = new TMovie() {
      Name = "other movie",
      Description = "Bad movie",
      Size = 4321,
      CreationYear = 1989
    };
    Target.Add(Media2);

    TraceMessage("Target after two additions", Target.ToString());

    Assert.AreEqual(2, Target.GetAll().Count());

    Target.Remove();

    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMediaSearchDatabaseJson_AddItems_ThenCommit() {
    TMediaSearchDatabaseJson Target = new TMediaSearchDatabaseJson() {
      Name = "test",
      Description = "test db",
      StoragePath = Path.GetTempPath(),
      StorageFilename = $"TestAdd{Random.Shared.Next()}.json"
    };

    Target.Open();
    Assert.IsTrue(Target.Exists());

    Target.Add(new TMovie() {
      Name = "test movie",
      Description = "Good movie",
      Size = 1234,
      CreationYear = 1966
    });

    Target.Add(new TMovie() {
      Name = "other movie",
      Description = "Bad movie",
      Size = 4321,
      CreationYear = 1989
    });

    TraceMessage("Target after two additions", Target.ToString());

    await Target.SaveAsync(CancellationToken.None);
    await Target.CloseAsync(CancellationToken.None);

    TraceMessage("RwContent of file", await File.ReadAllTextAsync(Target.FullStorageFilename));
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public async Task TMediaSearchDatabaseJson_AddItems_WithAutoCommit_Async() {
    TMediaSearchDatabaseJson Target = new TMediaSearchDatabaseJson() {
      Name = "test",
      Description = "test db",
      StoragePath = Path.GetTempPath(),
      StorageFilename = $"TestAdd{Random.Shared.Next()}.json",
      AutoSave = true
    };

    Target.Open();
    Assert.IsTrue(Target.Exists());

    Target.Add(new TMovie() {
      Name = "test movie",
      Description = "Good movie",
      Size = 1234,
      CreationYear = 1966
    });

    TraceMessage("RawContent of file after 1 addition", await File.ReadAllTextAsync(Target.FullStorageFilename));

    Target.Add(new TMovie() {
      Name = "other movie",
      Description = "Bad movie",
      Size = 4321,
      CreationYear = 1989
    });

    TraceMessage("RawContent of file after 2 additions", await File.ReadAllTextAsync(Target.FullStorageFilename));

    TraceMessage("Target after two additions", Target.ToString());

    await Target.SaveAsync(CancellationToken.None);
    await Target.CloseAsync(CancellationToken.None);

    TraceMessage("RwContent of file", await File.ReadAllTextAsync(Target.FullStorageFilename));
    Target.Remove();
    Assert.IsFalse(Target.Exists());
  }

  [TestMethod]
  public void Duplicate_TMediaSearchDatabaseJson() {
    TMediaSearchDatabaseJson Source = new TMediaSearchDatabaseJson() {
      Name = "test",
      Description = "test db"
    };

    Source.Add(new TMovie() {
      Name = "test movie",
      Description = "Good movie",
      Size = 1234,
      CreationYear = 1966
    });

    Source.Add(new TMovie() {
      Name = "other movie",
      Description = "Bad movie",
      Size = 4321,
      CreationYear = 1989
    });

    TraceMessage("Source after two additions", Source.ToString());

    TMediaSearchDatabaseJson Target = new TMediaSearchDatabaseJson(Source) {
      Name = "second",
      Description = "Duplicated db"
    };

    TraceMessage("Target", Target.ToString());
    Assert.AreEqual(2, Target.GetAll().Count());
  }
}