namespace MediaSearch.Database;

public partial class TDatabaseJson {

  public override bool Open() {
    if (!Exists()) {
      return false;
    }

    Schema.Clear();
    foreach (string TableNameItem in Directory.EnumerateDirectories(DatabaseFullName)) {
      string TableName = TableNameItem.AfterLast(Path.DirectorySeparatorChar);
      Logger.LogDebug($"Found table {TableName.WithQuotes()}");
      ITableHeader? Header = TableReadHeader(TableName);
      if (Header is null) {
        return false;
      }
      ITable? Table = TTable.Create(Header.Name, Header.TableType, this);
      if (Table is null) {
        return false;
      }
      Schema.Add(Table);
    }
    IsOpened = true;
    return true;
  }

  public override bool Close() {
    Schema?.Dispose();
    IsOpened = false;
    return true;
  }


}
