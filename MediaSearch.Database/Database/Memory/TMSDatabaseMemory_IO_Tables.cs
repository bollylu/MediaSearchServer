namespace MediaSearch.Database;

public partial class TMSDatabaseMemory {

  public bool TableCreate(IMSTable table) {
    return true;
  }

  public bool TableExists(IMSTable table) {
    return true;
  }
  public bool TableExists(string table) {
    return true;
  }

  public bool TableCheck(IMSTable table) {
    return true;
  }
  public bool TableCheck(string table) {
    return true;
  }


  public bool TableRemove(IMSTable table) {
    return true;
  }
  public bool TableRemove(string table) {
    return true;
  }

  public IMSTableHeader? TableReadHeader(IMSTable table) {
    if (!TableExists(table)) {
      return null;
    }

    IMSTableHeader? Header = new TMSTableHeader();
    return Header;

  }
  public IMSTableHeader? TableReadHeader(string name) {
    if (!TableExists(name)) {
      return null;
    }

    IMSTableHeader? Header = new TMSTableHeader();
    return Header;
  }




  public bool Save(IMSTable table) {
    throw new NotImplementedException();
  }

}
