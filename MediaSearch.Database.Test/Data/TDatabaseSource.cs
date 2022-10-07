using BLTools.Text;

namespace MediaSearch.Test.Database;

public static class TDatabaseSource {

  public static TDatabaseJson CreateJsonTestDatabaseEmpty() {
    Message("Instanciate test database");
    TDatabaseJson Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };
    Dump(Database);

    Message("Create database");
    Assert.IsTrue(Database.Create());

    Assert.IsTrue(Database.Exists());

    return Database;
  }

  public static TDatabaseJson CreateJsonTestDatabaseWithOneTable() {
    Message("Instanciate test database");
    TDatabaseJson Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = $"{Random.Shared.Next()}" };

    Message("Create database");
    Assert.IsTrue(Database.Create());

    Message("Open the database");
    Assert.IsTrue(Database.Open());

    Message("Create table Movies");
    ITable MovieTable = TTableSource.CreateTestTable<IMovie>(Database, "Movies", 2);

    Message("Add records");
    foreach (IMovie RecordItem in TRecordSource.GetMovieRecords()) {
      Database.Write(MovieTable, RecordItem);
    }

    Message("Close the database");
    Database.Close();

    Dump(Database);

    return Database;
  }

  public static TDatabaseJson CreateJsonTestDatabaseWithMultipleTables(int tableCount, int indent = 0) {
    string IndentSpace = TextBox.Spaces(indent);
    string DbName = $"{Random.Shared.Next()}";
    const int RecordCount = 3;

    Message($"{IndentSpace}Instanciate test database {DbName.WithQuotes()}");
    TDatabaseJson Database = new TDatabaseJson() { RootPath = Path.GetTempPath(), Name = DbName };

    Message($"{IndentSpace}Create database");
    Assert.IsTrue(Database.Create());

    Message($"{IndentSpace}Open the database");
    Assert.IsTrue(Database.Open());

    for (int i = 1; i <= tableCount; i++) {
      Message($"{IndentSpace}=== Create table{i}");
      ITable MovieTable = TTableSource.CreateTestTable<IMovie>(Database, $"table{i}", indent + 2).WithRecords(3);
      Message($"{IndentSpace}Add {RecordCount} records");
      foreach (IMovie RecordItem in TRecordSource.GetMovieRecords(RecordCount)) {
        Database.Write(MovieTable, RecordItem);
      }
      Message($"{IndentSpace}=== Table{i} done.");
    }

    Message($"{IndentSpace}Close the database");
    Database.Close();

    Dump(Database);

    return Database;
  }


}
