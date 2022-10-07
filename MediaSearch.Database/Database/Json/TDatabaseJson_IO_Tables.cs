namespace MediaSearch.Database;

public partial class TDatabaseJson {

  public override bool TableCreate(ITable table) {
    try {
      string TableDirectory = Path.Join(DatabaseFullName, table.Name);
      if (Directory.Exists(TableDirectory)) {
        throw new ApplicationException($"Unable to create table {table.Name.WithQuotes()} : Directory {TableDirectory.WithQuotes()} already exists");
      }
      Directory.CreateDirectory(TableDirectory);
      table.Database = this;
      Schema.AddTable(table);
      TableWriteHeader(table);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to create table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }

  public override bool TableExists(string name) {
    try {
      string TableDirectory = Path.Join(DatabaseFullName, name);
      return Directory.Exists(TableDirectory);
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to verify table existence : {name.WithQuotes()}", ex);
      return false;
    }
  }

  public override bool TableRemove(string name) {
    try {
      string TableDirectory = Path.Join(DatabaseFullName, name);
      if (!Directory.Exists(TableDirectory)) {
        throw new ApplicationException($"Directory {TableDirectory} does not exist");
      }
      Directory.Delete(TableDirectory, true);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to remove table {name.WithQuotes()}", ex);
      return false;
    }
  }

  public override bool TableCheck(string name) {
    try {
      return TableExists(name) && File.Exists(Path.Join(DatabaseFullName, name, TABLE_HEADER_FILENAME));
    } catch {
      return false;
    }
  }

  public override ITableHeader? TableReadHeader(string name) {
    if (!TableExists(name)) {
      return null;
    }

    string HeaderFullname = Path.Join(DatabaseFullName, name, TABLE_HEADER_FILENAME);
    try {
      string RawContent = File.ReadAllText(HeaderFullname);
      JsonDocument JsonContent = JsonDocument.Parse(RawContent);
      string MediaTypeName = JsonContent.RootElement.GetProperty("MediaSource").GetPropertyEx("MediaType").GetString() ?? "";

      ITableHeader? TableHeader = TTableHeader.Create(MediaTypeName);
      if (TableHeader is null) {
        return null;
      }

      return (ITableHeader?)IJson.FromJson(TableHeader.GetType(), RawContent, SerializerOptions);
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to read header for table {name.WithQuotes()}", ex);
      return null;
    }
  }

  public override bool TableWriteHeader(ITable table) {

    if (!TableExists(table.Name)) {
      Logger.LogErrorBox($"Unable to write header for table {table.Name.WithQuotes()}", "Table is missing");
      return false;
    }

    string HeaderFullname = Path.Join(DatabaseFullName, table.Name, TABLE_HEADER_FILENAME);
    try {
      string RawContent = IJson.ToJson(table.Header, SerializerOptions);
      File.WriteAllText(HeaderFullname, RawContent);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to write header for table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }

  public override bool TableReindex(string table) {
    throw new NotImplementedException();
  }

  public override IEnumerable<ITable> TableList() {
    foreach (string TablePathItem in Directory.EnumerateDirectories(DatabaseFullName).Where(d => !d.EndsWith(TABLE_HEADER_FILENAME))) {
      string TableName = Path.GetFileName(TablePathItem);
      ITableHeader? Header = TableReadHeader(TableName);
      if (Header is not null) {
        ITable? Table = TTable.Create(TableName, Header.TableType);
        if (Table is not null) {
          yield return Table;
        }
      }
    }
  }

  public override IEnumerable<ITable> TableSystemList() {
    throw new NotImplementedException();
  }
}
