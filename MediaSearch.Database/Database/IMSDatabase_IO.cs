namespace MediaSearch.Database;

public partial interface IMSDatabase {

  bool Save(IMSTable table);

  bool Save(IMSTable table, IMSRecord record);

  RECORD Get<RECORD>(string table, string key) where RECORD : IMSRecord;

}
