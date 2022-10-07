
using BLTools.Text;

namespace MediaSearch.Test.Database;

public static class TTableSource {

  public static ITable<RECORD> InstanciateRandomTable<RECORD>(int indent = 0) where RECORD : class, IRecord {
    string IndentSpace = TextBox.Spaces(indent);
    Message($"{IndentSpace}Instanciate a new table");
    Message("Instanciate a new table");
    ITable<RECORD>? RetVal = TTable.Create<RECORD>(Random.Shared.Next().ToString());
    Assert.IsNotNull(RetVal);
    Dump(RetVal);
    return RetVal;
  }

  public static ITable<RECORD> CreateTestTable<RECORD>(IDatabase database, string name, int indent = 0) where RECORD : class, IRecord {
    string IndentSpace = TextBox.Spaces(indent);
    Message($"{IndentSpace}Instanciate a new table");
    ITable<RECORD>? Table = TTable.Create<RECORD>(string.IsNullOrWhiteSpace(name) ? Random.Shared.Next().ToString() : name);
    Assert.IsNotNull(Table);
    Message($"{IndentSpace}Create the table {Table.Name.WithQuotes()} in database {database.Name.WithQuotes()}");
    database.TableCreate(Table);
    return Table;
  }

}
