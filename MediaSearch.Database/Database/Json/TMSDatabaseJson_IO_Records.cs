namespace MediaSearch.Database;

public partial class TMSDatabaseJson {

  public override bool Write(IMSTable? table, IMSRecord record) {
    if (table is null) {
      Logger.LogErrorBox($"Unable to write record : Table is null", record);
      return false;
    }

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
      string RecordName = Path.Join(DatabaseFullName, table.Name, record.ID);
      File.WriteAllText(RecordName, RawData);
      return true;
    } catch (Exception ex) {
      Logger.LogErrorBox($"Unable to write record in table {table.Name.WithQuotes()}", ex);
      return false;
    }
  }



  public override RECORD Read<RECORD>(IMSTable table, string key) {
    throw new NotImplementedException();
  }

  public override RECORD Read<RECORD>(string table, string key) {
    throw new NotImplementedException();
  }

}
