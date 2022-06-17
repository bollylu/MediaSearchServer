namespace MediaSearch.Database;

public class TMSTable : AMSTable<IMSRecord> {

  public static IMSTable? Create(string tableName, Type tableType, IMSDatabase database) {
    return tableType.Name switch {
      nameof(IMovie) => new TMSTableMovie(tableName, database),
      _ => throw new ApplicationException($"Unable to create Table of type {tableType.Name}"),
    };
  }

  public static IMSTable? Create<RECORD>(string tableName, IMSDatabase database) where RECORD : class, IMSRecord {
    return typeof(RECORD).Name switch {
      nameof(IMovie) => new TMSTableMovie(tableName, database),
      _ => throw new ApplicationException($"Unable to create Table of type {typeof(RECORD).Name}"),
    };
  }

}

public class TMSTableMovie : AMSTable<IMovie>, IMSTable<IMovie>, IMSTableMovie {

  public TMSTableMovie() { }
  public TMSTableMovie(string tableName, IMSDatabase database) : base(tableName, database) { }

}
