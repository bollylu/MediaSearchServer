namespace MediaSearch.Database;

public partial class TDatabaseJson {

  public override bool Open() {
    if (!Exists()) {
      return false;
    }

    if (Schema.Exists()) {
      IsOpened = Schema.Read();
    } else {
      IsOpened = Schema.Build();
    }

    return IsOpened;

    //foreach (string TableNameItem in Directory.EnumerateDirectories(DatabaseFullName)) {
    //  string TableName = TableNameItem.AfterLast(Path.DirectorySeparatorChar);
    //  Logger.LogDebug($"Found table {TableName.WithQuotes()}");
    //  ITableHeader? Header = TableReadHeader(TableName);
    //  if (Header is null) {
    //    return false;
    //  }
    //  ITable? Table = TTable.Create(Header.Name, Header.TableType, this);
    //  if (Table is null) {
    //    return false;
    //  }
    //  Schema.Add(Table);
    //}
  }

  public override bool Close() {
    Schema.Save();
    IsOpened = false;
    return true;
  }


}
