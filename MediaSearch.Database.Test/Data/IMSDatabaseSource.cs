namespace MediaSearch.Test.Database;

public static class IMSDatabaseSource {

  public static TMSDatabaseJson CreateJsonTestDatabaseEmpty() {
    Message("Instanciate test database");
    TMSDatabaseJson Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Database);

    Message("Create database");
    Assert.IsTrue(Database.Create());

    Assert.IsTrue(Database.Exists());

    return Database;
  }

  public static TMSDatabaseJson CreateJsonTestDatabase() {
    Message("Instanciate test database");
    TMSDatabaseJson Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Database);

    Message("Create Table Movies and record to insert into");
    IMSTableMovie MovieTable = new TMSTableMovie() { Name = "Movies" };

    Message("Create database");
    Assert.IsTrue(Database.Create());

    Message("Open the database");
    Assert.IsTrue(Database.Open());

    Message("Create table Movies in database");
    Assert.IsTrue(Database.TableCreate(MovieTable));

    Message("Add records");
    foreach (IMovie RecordItem in IMSRecordSource.GetMovieRecords()) {
      MovieTable.Add(RecordItem);
    }

    Message("Close the database");
    Database.Close();

    return Database;
  }
}
