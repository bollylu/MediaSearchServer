namespace MediaSearch.Storage.Test;

[TestClass]
public class TStorageSqLiteTests {
  [TestMethod]
  public void Instanciate_TStorageMovieSqlite() {

    IStorage Storage = new TStorageMovieSqlite(Path.GetTempPath(), "movie");
    Dump(Storage);

    Ok();
  }

  [TestMethod]
  public void TStorageMovieSqlite_CreateDb() {

    IStorage Storage = new TStorageMovieSqlite(Path.GetTempPath(), Random.Shared.Next().ToString());
    Dump(Storage);

    Message("Db is not yet present");
    Assert.IsFalse(Storage.Exists());

    Message("Creating db");
    Assert.IsTrue(Storage.Create());
    Dump(Storage);

    Message("Db is present");
    Assert.IsTrue(Storage.Exists());

    Message("Cleanup");
    Assert.IsTrue(Storage.Remove());
    Assert.IsFalse(Storage.Exists());

    Ok();
  }
}