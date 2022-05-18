namespace MediaSearch.Database;

public partial class TMSDatabaseJson {

  public override bool Write(IMSTable table, IMSRecord record) {
    if (!IsOpened) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()} : Database must be opened first", record);
      return false;
    }
    if (!TableExists(table)) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()} : Table does not exist", record);
      return false;
    }

    if (record is not IJson JsonRecord) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()} : Record is not IJson", record);
      return false;
    }

    try {
      string RawData = JsonRecord.ToJson();
      string RecordName = Path.Join(DatabaseFullName, table.Name, $"{record.ID}.json");
      File.WriteAllText(RecordName, RawData);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }

  public override RECORD Read<RECORD>(IMSTable table, string key) where RECORD : struct {
    if (!IsOpened) {
      Logger.LogErrorBox($"Unable to read record from table {table.Name.WithQuotes()} : Database must be opened first", key);
      throw new ApplicationException("Database is not opened");
    }
    if (!TableExists(table)) {
      Logger.LogErrorBox($"Unable to read record from table {table.Name.WithQuotes()} : Table does not exist", key);
      throw new ApplicationException("Table does not exist");
    }

    try {
      string RecordName = Path.Join(DatabaseFullName, table.Name, $"{key}.json");
      string RawContent = File.ReadAllText(RecordName);
      RECORD RetVal = IJson<RECORD>.FromJson(RawContent);
      if (RetVal is null) {
        throw new JsonException("Unable to convert record");
      }
      return RetVal;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to read record from table {table.Name.WithQuotes()}", ex);
    }
  }

  public override RECORD Read<RECORD>(string table, string key) {
    IMSTable? Table = GetTable(table);
    if (Table is null) {
      Logger.LogError("Unable to read record from table : table name is missing");
      throw new ArgumentException("Unable to read record from table : table name is missing", table);
    }
    return Read<RECORD>(Table, key);
  }

  public override string RecordDump(IMSTable table, string recordId) {
    try {
      string RecordFullName = Path.Join(DatabaseFullName, table.Name, $"{recordId}.json");
      return File.ReadAllText(RecordFullName);
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to dump record {recordId.WithQuotes()} for table {table.Name.WithQuotes()}", ex);
      return "";
    }
  }
}
