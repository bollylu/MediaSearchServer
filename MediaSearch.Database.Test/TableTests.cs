namespace MediaSearch.Test.Database;

[TestClass]
public class TableTests {

  [TestMethod]
  public void Instanciate_TMSTable_IMovie_Empty() {
    Message("Instanciate empty IMovie table");
    IMSTable Target = IMSTableSource.InstanciateRandomTable();
    Dump(Target);
    Assert.IsNotNull(Target);
  }

  [TestMethod]
  public void TMSTable_IMovie_WriteHeader() {
    Message("Create a Json database");
    TMSDatabaseJson Database = IMSDatabaseSource.CreateJsonTestDatabaseEmpty();
    Message("Create an IMovie table");
    IMSTable Table = IMSTableSource.CreateTestTable(Database);
    Message("Write the table header");
    Assert.IsTrue(Database.TableWriteHeader(Table));
    Dump(Table);
    DumpWithMessage("Header raw value", File.ReadAllText(Path.Join(Database.DatabaseFullName, Table.Name, TMSDatabaseJson.TABLE_HEADER_FILENAME)));
    Assert.IsTrue(Database.Remove());
    Ok();

  }

  [TestMethod]
  public void TMSTable_IMovie_SetMediaSource() {
    Message("Instanciate IMovie table and add Mediasource");
    IMSTable Target = new TMSTableMovie() { Name = "Movie table" };
    Assert.IsNotNull(Target);
    Assert.IsNotNull(Target.Header);
    IMediaSource Source = new TMediaSourceMovie("\\\\andromeda.sharenet.priv\\movies");
    Target.Header.SetMediaSource(Source);
    Dump(Target);
  }

  [TestMethod]
  public void TMSTable_IMovie_SetInvalidMediaSource() {
    Message("Instanciate IMovie table and add invalid Mediasource");
    IMSTable Target = new TMSTableMovie() { Name = "Movie table" };
    Dump(Target);
    Assert.IsNotNull(Target);
    Assert.IsNotNull(Target.Header);

    IMediaSource? Source = TMediaSource.Create("\\\\andromeda.sharenet.priv\\movies", typeof(IMedia));
    Dump(Source);
    Assert.IsNotNull(Source);
    Assert.IsFalse(Target.Header.SetMediaSource(Source));
  }

}