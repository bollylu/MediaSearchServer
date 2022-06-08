namespace MediaSearch.Database.Test;

[TestClass]
public class TMSRecordTests {
  private static IMSDatabase CreateTestDatabase() {
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };

    IMovie Record1 = new TMovie("Wargames", 1986);
    Record1.Descriptions.Add(ELanguage.French, "Un gamin joue avec un ordi");

    IMovie Record2 = new TMovie("Jeux interdits", 1959);
    Record2.Descriptions.Add(ELanguage.French, "Des enfants et des morts");

    IMovie Record3 = new TMovie("Top gun", 1988);
    Record3.Descriptions.Add(ELanguage.French, "Des pilotes et des avions");

    IMovie Record4 = new TMovie("Le hobbit", 2012);
    Record4.Descriptions.Add(ELanguage.French, "Un monde de fantaisie");

    Assert.IsTrue(Database.Create());
    Assert.IsTrue(Database.Open());

    Assert.IsTrue(Database.TableCreate(MovieTable));

    MovieTable.Add(Record1);
    MovieTable.Add(Record2);
    MovieTable.Add(Record3);
    MovieTable.Add(Record4);

    Database.Close();

    return Database;
  }

  [TestMethod]
  public void Instanciate_TMSRecord() {
    Message("Create Record");
    IMSRecord Target = new TMovie();
    Dump(Target);
  }

  [TestMethod]
  public void TMSRecord_Write_JsonDatabase() {
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };
    IMovie Record = new TMovie();

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
    Database.Write(MovieTable, Record);

    DumpWithMessage("Table content", Directory.GetFiles(Path.Join(Database.DatabaseFullName, MovieTable.Name), "*.*"));

    DumpWithMessage("Raw record", File.ReadAllText(Path.Join(Database.DatabaseFullName, MovieTable.Name, $"{Record.ID}.json")));

    Message("Closing and cleanup");
    Database.Close();
    Database.Remove();
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
    IMSDatabase Database = CreateTestDatabase();
    DumpWithMessage("Raw database", Database.Dump());

    Database.Remove();
  }

  [TestMethod]
  public void TMSRecord_ReadRecord_JsonDatabase() {
    IMSDatabase Database = CreateTestDatabase();
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
