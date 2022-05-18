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
    TraceMessage("Create Record");
    IMSRecord Target = new TMovie();
    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
  }

  [TestMethod]
  public void TMSRecord_Write_JsonDatabase() {
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };
    IMovie Record = new TMovie();

    TraceMessage("Creating database Json");
    Assert.IsTrue(Database.Create());
    Assert.IsTrue(Database.Exists());

    TraceMessage("Opening database Json");
    Assert.IsTrue(Database.Open());
    TraceBox($"{nameof(Database)} : {Database.GetType().Name}", Database);

    TraceMessage("Creating table");
    Assert.IsTrue(Database.TableCreate(MovieTable));
    TraceBox($"{nameof(MovieTable)} : {MovieTable.GetType().GetGenericName()}", MovieTable);

    TraceBox("Write record", Record);
    Database.Write(MovieTable, Record);

    TraceBox("Table content", Directory.GetFiles(Path.Join(Database.DatabaseFullName, MovieTable.Name), "*.*"));

    TraceBox("Raw record", File.ReadAllText(Path.Join(Database.DatabaseFullName, MovieTable.Name, $"{Record.ID}.json")));

    TraceMessage("Closing and cleanup");
    Database.Close();
    Database.Remove();
  }

  [TestMethod]
  public void TMSRecord_Write_JsonDatabaseTable() {
    IMSDatabase Database = new TMSDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>() { Name = "Movies" };
    IMovie Record = new TMovie("Wargames", 1986);
    Record.Descriptions.Add(ELanguage.French, "Un gamin joue avec un ordi");

    TraceMessage("Creating database Json");
    Assert.IsTrue(Database.Create());
    Assert.IsTrue(Database.Exists());

    TraceMessage("Opening database Json");
    Assert.IsTrue(Database.Open());
    TraceBox($"{nameof(Database)} : {Database.GetType().Name}", Database);

    TraceMessage("Creating table");
    Assert.IsTrue(Database.TableCreate(MovieTable));
    TraceBox($"{nameof(MovieTable)} : {MovieTable.GetType().GetGenericName()}", MovieTable);

    TraceBox("Write record", Record);
    MovieTable.Add(Record);

    TraceBox("Raw database", Database.Dump());
    TraceBox("Raw record", Database.RecordDump(MovieTable, Record.ID));

    TraceMessage("Closing and cleanup");
    Database.Close();
    Database.Remove();
  }

  [TestMethod]
  public void TMSRecord_Dump_JsonDatabase() {
    IMSDatabase Database = CreateTestDatabase();
    TraceBox("Raw database", Database.Dump());
    
    Database.Remove();
  }

  [TestMethod]
  public void TMSRecord_ReadRecord_JsonDatabase() {
    IMSDatabase Database = CreateTestDatabase();
    TraceBox($"{nameof(Database)} : {Database.GetType().Name}", Database);

    TraceBox("Raw database", Database.Dump());
    Database.Open();

    IMovie? Target = Database.Read<TMovie>("movies", "4C6E47324F65594C6E75314A55663631486C384A446B516E7871677830424844324C30577A30646E4359593D");
    Assert.IsNotNull(Target);

    Database.Close();

    TraceBox($"{nameof(Target)} : {Target.GetType().Name}", Target);
    Database.Remove();
  }
}
