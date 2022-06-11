using MediaSearch.Database;

namespace MediaSearch.Test.Database;

[TestClass]
public class TMSRecordTests {

  [TestMethod]
  public void Instanciate_TMSRecord() {
    Message("Create IMSRecord");
    IMSRecord Target = new TMovie();
    Dump(Target);
    Assert.IsNotNull(Target);
    Ok();
  }

  [TestMethod]
  public void TMSRecord_Write_JsonDatabase() {
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };

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

      IMSRecord Record = IMSRecordSource.GetMovieRecord("Hello Dolly", "Nice musical", 1956);
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
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };
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
    IMSDatabase Database = IMSDatabaseSource.CreateTestDatabase();
    Dump(Database);

    Message("Verify that database exists");
    Assert.IsTrue(Database.Exists());

    Message("Remove database");
    Database.Remove();

    Ok();
  }

  [TestMethod]
  public void TMSRecord_ReadRecord_JsonDatabase() {
    IMSDatabase Database = IMSDatabaseSource.CreateTestDatabase();
    Dump(Database);

    DumpWithMessage("Raw database", Database.Dump());
    Database.Open();

    IMovie? Target = Database.Read<TMovie>("movies", "4C6E47324F65594C6E75314A55663631486C384A446B516E7871677830424844324C30577A30646E4359593D");
    Assert.IsNotNull(Target);

    Database.Close();

    Dump(Target);
    Database.Remove();
  }
}
