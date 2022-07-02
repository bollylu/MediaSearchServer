namespace MediaSearch.Database;

public partial class TDatabaseMemory {

  public override bool TableCreate(ITable table) {
    return true;
  }

  public override bool TableExists(string table) {
    return true;
  }

  public override bool TableCheck(string table) {
    return true;
  }

  public override bool TableRemove(string table) {
    return true;
  }

  public override ITableHeader? TableReadHeader(ITable table) {
    if (!TableExists(table)) {
      return null;
    }

    ITableHeader? Header = TTableHeader.Create(table.Name, typeof(IRecord));
    return Header;

  }
  public override ITableHeader? TableReadHeader(string name) {
    if (!TableExists(name)) {
      return null;
    }

    ITableHeader? Header = TTableHeader.Create(name, typeof(IRecord));
    return Header;
  }

  public override bool TableWriteHeader(ITable table) {

    if (!TableExists(table.Name)) {
      Logger.LogErrorBox($"Unable to write header for table {table.Name.WithQuotes()}", "Table is missing");
      return false;
    }

    return true;
  }

  public override bool TableReindex(string table) {
    throw new NotImplementedException();
  }

  public override IEnumerable<ITable> TableList() {
    throw new NotImplementedException();
  }

  public override IEnumerable<ITable> TableSystemList() {
    throw new NotImplementedException();
  }
}
