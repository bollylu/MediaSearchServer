namespace MediaSearch.Test.Database;

[TestClass]
public class TMSRecordTests {

  [TestMethod]
  public void Instanciate_TMSRecord() {
    Message("Create IMSRecord");
    IRecord Target = new TMovie();
    Dump(Target);
    Assert.IsNotNull(Target);
    Ok();
  }

  [TestMethod]
  public void TMSRecord_Write_JsonDatabase() {
    TDatabaseJson Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    ITable MovieTable = new TTableMovie() { Name = "Movies" };

    Message("Creating database Json");
    Assert.IsTrue(Database.Create());
    Assert.IsTrue(Database.Exists());

    try {
      Message("Opening database Json");
      Assert.IsTrue(Database.Open());
      Dump(Database);

      Message("Creating table");
      Assert.IsTrue(Database.TableCreate(MovieTable));
      Dump(MovieTable);

      IRecord Record = TRecordSource.GetMovieRecord("Hello Dolly", "Nice musical", 1956);
      Message("Write record");
      Dump(Record);
      Database.Write(MovieTable, Record);

      Dump(MovieTable);

      DumpWithMessage("Table content", Directory.GetFiles(Path.Join(Database.DatabaseFullName, MovieTable.Name), "*.*"));

      string RawRecord = File.ReadAllText(Path.Join(Database.DatabaseFullName, MovieTable.Name, $"{Record.ID}.json"));
      Dump(RawRecord);
      Assert.AreNotEqual("{}", RawRecord);
    } finally {
      Message("Closing and cleanup");
      Database.Close();
      Database.Remove();
    }

    Ok();
  }

  [TestMethod]
  public void TMSRecord_Write_JsonDatabaseTable() {
    IDatabase Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTableMovie MovieTable = new TTableMovie() { Name = "Movies" };
    IMovie Record = new TMovie("Wargames", 1986);
    Record.Descriptions.Add(ELanguage.French, "Un gamin joue avec un ordi");

    Message("Creating database Json");
    Assert.IsTrue(Database.Create());
    Assert.IsTrue(Database.Exists());

    Message("Opening database Json");
    Assert.IsTrue(Database.Open());
    Dump(Database);

    Message("Creating table");
    Assert.IsTrue(Database.TableCreate(MovieTable));
    Dump(MovieTable);

    DumpWithMessage("Write record", Record);
    MovieTable.Add(Record);

    DumpWithMessage("Raw database", Database.Dump());
    DumpWithMessage("Raw record", Database.RecordDump(MovieTable, Record.ID));

    Message("Closing and cleanup");
    Database.Close();
    Database.Remove();
  }

  [TestMethod]
  public void TMSRecord_Dump_JsonDatabase() {
    TDatabaseJson Database = TDatabaseSource.CreateJsonTestDatabase();
    Dump(Database);

    Message("Verify that database exists");
    Assert.IsTrue(Database.Exists());

    DumpWithMessage("Table list", Database.TableList());

    Message("Remove database");
    Database.Remove();

    Ok();
  }

  [TestMethod]
  public void TMSRecord_ReadRecord_JsonDatabase() {
    TDatabaseJson Database = TDatabaseSource.CreateJsonTestDatabase();
    Dump(Database);

    Assert.IsTrue(Database.Close());

    Message("=-=-=-=-=-=-=--==--====-");
    DumpWithMessage("Raw database", Database.Dump());
    Message("=-=-=-=-=-=-=--==--====-");

    Database.Open();

    IMovie? Target = Database.Read<TMovie>("movies", TRecordSource.GetMovieRecords().First().ID);
    Assert.IsNotNull(Target);

    Assert.IsTrue(Database.Close());

    Dump(Target);
    Database.Remove();
  }
}
