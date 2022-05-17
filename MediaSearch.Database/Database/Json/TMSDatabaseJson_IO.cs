namespace MediaSearch.Database;

public partial class TMSDatabaseJson {

  public override bool Open() {
    if (!Exists()) {
      return false;
    }

    foreach (string TableNameItem in Directory.EnumerateDirectories(DatabaseFullName)) {
      Logger.LogDebug($"Found table {TableNameItem.AfterLast('\\').WithQuotes()}");
      AddTable(new TMSTable(TableNameItem));
    }
    IsOpened = true;
    return true;
  }

  public override bool Close() {
    IsOpened = false;
    return true;
  }

  
}
