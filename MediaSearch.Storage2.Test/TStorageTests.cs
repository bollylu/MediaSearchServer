namespace MediaSearch.Storage.Test;

//[TestClass]
//public class TStorageSqLiteTests {
//  [TestMethod]
//  public void Instanciate_TStorageMovieSqlite() {

//    IStorage Storage = new TStorageMovieSqlite(Path.GetTempPath(), "movie");
//    Dump(Storage);

//    Ok();
//  }

//  [TestMethod]
//  public async Task TStorageMovieSqlite_CreateDb() {

//    IStorage Storage = new TStorageMovieSqlite(Path.GetTempPath(), Random.Shared.Next().ToString());
//    Dump(Storage);

//    Message("Db is not yet present");
//    Assert.IsFalse(await Storage.Exists());

//    Message("Creating db");
//    Assert.IsTrue(await Storage.Create());
//    Dump(Storage);

//    Message("Db is present");
//    Assert.IsTrue(await Storage.Exists());

//    Message("Cleanup");
//    Assert.IsTrue(await Storage.Remove());
//    Assert.IsFalse(await Storage.Exists());

//    Ok();
//  }
//}