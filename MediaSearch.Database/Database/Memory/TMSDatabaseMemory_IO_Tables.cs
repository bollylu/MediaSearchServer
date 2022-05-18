namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public override bool TableCreate(IMSTable table) {
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

  public override IMSTableHeader? TableReadHeader(IMSTable table) {
    if (!TableExists(table)) {
      return null;
    }

    IMSTableHeader? Header = new TMSTableHeader();
    return Header;

  }
  public override IMSTableHeader? TableReadHeader(string name) {
    if (!TableExists(name)) {
      return null;
    }

    IMSTableHeader? Header = new TMSTableHeader();
    return Header;
  }

  public override bool TableWriteHeader(IMSTable table) {

    if (!TableExists(table.Name)) {
      Logger.LogErrorBox($"Unable to write header for table {table.Name.WithQuotes()}", "Table is missing");
      return false;
    }

    return true;
  }

  public override bool TableReindex(string table) {
    throw new NotImplementedException();
  }
}
