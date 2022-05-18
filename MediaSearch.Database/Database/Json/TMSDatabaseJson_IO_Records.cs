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

  public override RECORD? Read<RECORD>(IMSTable table, string key) where RECORD : class {
    if (!IsOpened) {
      Logger.LogErrorBox($"Unable to read record from table {table.Name.WithQuotes()} : Database must be opened first", key);
      return null;
    }
    if (!TableExists(table)) {
      Logger.LogErrorBox($"Unable to read record from table {table.Name.WithQuotes()} : Table does not exist", key);
      return null;
    }

    try {
      string RecordName = Path.Join(DatabaseFullName, table.Name, $"{key}.json");
      string RawContent = File.ReadAllText(RecordName);
      RECORD? RetVal = IJson<RECORD>.FromJson(RawContent);
      
      return RetVal;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to read record from table {table.Name.WithQuotes()}", ex);
      return null;
    }
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
