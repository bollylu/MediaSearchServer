using MediaSearch.Database;

namespace MediaSearch.Test.Database;

public static class IMSDatabaseSource {

  public static IMSDatabase CreateTestDatabase() {
    Message("Create test database in memory");
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Database);

    Message("Create Table Movies and record to insert into");
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };

    Message("Create database for real");
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
