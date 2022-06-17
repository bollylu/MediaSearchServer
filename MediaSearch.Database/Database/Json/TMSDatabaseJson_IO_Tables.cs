namespace MediaSearch.Database;

public partial class TMSDatabaseJson {

  public override bool TableCreate(IMSTable table) {
    try {
      string TableDirectory = Path.Join(DatabaseFullName, table.Name);
      if (Directory.Exists(TableDirectory)) {
        throw new ApplicationException($"Unable to create table {table.Name.WithQuotes()} : Directory {TableDirectory.WithQuotes()} already exists");
      }
      Directory.CreateDirectory(TableDirectory);
      Schema.Add(table);
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

  public override IMSTableHeader? TableReadHeader(string name) {
    if (!TableExists(name)) {
      return null;
    }

    string HeaderFullname = Path.Join(DatabaseFullName, name, TABLE_HEADER_FILENAME);
    try {
      string RawContent = File.ReadAllText(HeaderFullname);
      JsonDocument JsonContent = JsonDocument.Parse(RawContent);
      string MediaTypeName = JsonContent.RootElement.GetProperty("MediaSource").GetPropertyEx("MediaType").GetString() ?? "";
      Type? MediaType = Type.GetType($"{nameof(MediaSearch)}.{nameof(MediaSearch.Models)}.{MediaTypeName},{nameof(MediaSearch)}.{nameof(MediaSearch.Models)}");
      if (MediaType is null) {
        throw new Exception($"Unable to read header : invalid media type");
      }
      Type TableType = typeof(AMSTableHeader<>).MakeGenericType(MediaType);
      return (IMSTableHeader?)IJson.FromJson(TableType, RawContent, SerializerOptions);
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to read header for table {name.WithQuotes()}", ex);
      return null;
    }
  }

  public override bool TableWriteHeader(IMSTable table) {

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

  public override IEnumerable<IMSTableGeneric> TableList() {
    foreach (string TablePathItem in Directory.EnumerateDirectories(DatabaseFullName).Where(d => !d.EndsWith(TABLE_HEADER_FILENAME))) {
      //yield return new AMSTable(Path.GetFileName(TablePathItem));
    }
    yield break;
  }

  public override IEnumerable<IMSTableGeneric> TableSystemList() {
    throw new NotImplementedException();
  }
}
