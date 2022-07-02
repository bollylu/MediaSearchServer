namespace MediaSearch.Test.Database;

public static class TTableSource {

  public static ITable InstanciateRandomTable<RECORD>() where RECORD : class, IRecord {
    Message("Instanciate a new table");
    ITable? RetVal = TTable.Create<RECORD>(Random.Shared.Next().ToString());
    Assert.IsNotNull(RetVal);
    Dump(RetVal);
    return RetVal;
  }

  public static ITable CreateTestTable<RECORD>(IDatabase database, string name) where RECORD : class, IRecord {
    Message("Instanciate a new table");
    ITable? Table = TTable.Create<RECORD>(string.IsNullOrWhiteSpace(name) ? Random.Shared.Next().ToString() : name);
    Assert.IsNotNull(Table);
    Dump(Table);
    Message($"Create the table {Table.Name.WithQuotes()} in database {database.Name.WithQuotes()}");
    database.TableCreate(Table);
    return Table;
  }

}
