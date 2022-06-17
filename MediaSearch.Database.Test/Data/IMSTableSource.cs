namespace MediaSearch.Test.Database;

public static class IMSTableSource {

  public static IMSTable<IMSRecord> InstanciateRandomTable() {
    Message("Instanciate a new table");
    IMSTable<IMSRecord> RetVal = new TMSTable() { Name = $"{Random.Shared.Next()}" };
    Dump(RetVal);
    return RetVal;
  }

  public static IMSTable<IMSRecord> CreateTestTable(IMSDatabase database) {
    Message("Instanciate a new table");
    IMSTable<IMSRecord> Table = new TMSTable() { Name = $"{Random.Shared.Next()}" };
    Table.Database = database;
    Dump(Table);
    Message($"Create the table {Table.Name.WithQuotes()} in database {database.Name.WithQuotes()}");
    database.TableCreate(Table);
    return Table;
  }

}
