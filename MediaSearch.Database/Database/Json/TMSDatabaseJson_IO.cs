namespace MediaSearch.Database;

public partial class TMSDatabaseJson {

  public override bool Open() {
    if (!Exists()) {
      return false;
    }

    foreach (string TableNameItem in Directory.EnumerateDirectories(DatabaseFullName)) {
      string TableName = TableNameItem.AfterLast(Path.DirectorySeparatorChar);
      Logger.LogDebug($"Found table {TableName.WithQuotes()}");
      IMSTableHeader? Header = TableReadHeader(TableName);
      if (Header is null) {
        return false;
      }
      IMSTable? Table = TMSTable.Create(Header.Name, Header.TableType, this);
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
