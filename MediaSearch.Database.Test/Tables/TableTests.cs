namespace MediaSearch.Test.Database;

[TestClass]
public class TableTests {

  [TestMethod]
  public void Instanciate_TMSTable_IMovie_Empty() {
    Message("Instanciate empty IMovie table");
    ITable Target = TTableSource.InstanciateRandomTable<IMovie>();
    Dump(Target);
    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void TMSTable_IMovie_WriteHeader() {
    Message("Create a Json database");
    TDatabaseJson Database = TDatabaseSource.CreateJsonTestDatabaseEmpty();

    Message("Create an IMovie table");
    ITable Table = TTableSource.CreateTestTable<IMovie>(Database, "Movies");

    Message("Write the table header");
    Assert.IsTrue(Database.TableWriteHeader(Table));

    Dump(Table);

    DumpWithMessage("Header raw value", File.ReadAllText(Path.Join(Database.DatabaseFullName, Table.Name, TDatabaseJson.TABLE_HEADER_FILENAME)));

    Assert.IsTrue(Database.Remove());

    Ok();

  }

  [TestMethod]
  public void TMSTable_IMovie_SetMediaSource() {
    Message("Instanciate IMovie table and add Mediasource");
    ITable Target = new TTableMovie() { Name = "Movie table" };
    Assert.IsNotNull(Target);
    Assert.IsNotNull(Target.Header);
    IMediaSource Source = new TMediaSourceMovie("\\\\andromeda.sharenet.priv\\movies");
    Target.Header.SetMediaSource(Source);
    Dump(Target);
  }


}