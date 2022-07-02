namespace MediaSearch.Test.Database;

public static class TDatabaseSource {

  public static TDatabaseJson CreateJsonTestDatabaseEmpty() {
    Message("Instanciate test database");
    TDatabaseJson Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Database);

    Message("Create database");
    Assert.IsTrue(Database.Create());

    Assert.IsTrue(Database.Exists());

    return Database;
  }

  public static TDatabaseJson CreateJsonTestDatabase() {
    Message("Instanciate test database");
    TDatabaseJson Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };

    Message("Create database");
    Assert.IsTrue(Database.Create());

    Message("Open the database");
    Assert.IsTrue(Database.Open());

    Message("Create table Movies");
    ITable MovieTable = TTableSource.CreateTestTable<IMovie>(Database, "Movies");

    Message("Add records");
    foreach (IMovie RecordItem in TRecordSource.GetMovieRecords()) {
      Database.Write(MovieTable, RecordItem);
    }

    Message("Close the database");
    Database.Close();

    Dump(Database);

    return Database;
  }
}
