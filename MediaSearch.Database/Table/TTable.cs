namespace MediaSearch.Database;

public static class TTable {

  public static ITable? Create(string tableName, Type tableType) {
    return Create(tableName, tableType.Name);
  }

  public static ITable? Create<RECORD>(string tableName) where RECORD : class, IRecord {
    return Create(tableName, typeof(RECORD).Name);
  }

  public static ITable? Create(string tableName, string tableType) {
    return tableType switch {
      nameof(IMovie) => new TTable.Create<IMovie>(tableName),
      _ => throw new ApplicationException($"Unable to create Table of type {tableType}"),
    };
  }


  public static ITable? Create(string tableName, Type tableType, IDatabase database) {
    return Create(tableName, tableType.Name, database);
  }

  public static ITable? Create<RECORD>(string tableName, IDatabase database) where RECORD : class, IRecord {
    return Create(tableName, typeof(RECORD).Name, database);
  }

  public static ITable? Create(string tableName, string tableType, IDatabase database) {
    return tableType switch {
      nameof(IMovie) => new TTableMovie(tableName, database),
      _ => throw new ApplicationException($"Unable to create Table of type {tableType}"),
    };
  }

}

//public class TTableMovie : ATable<IMovie>, ITable<IMovie>, ITable {

//  public TTableMovie() { }
//  public TTableMovie(string tableName) : base(tableName) {

//  }
//  public TTableMovie(string tableName, IDatabase database) : base(tableName, database) {

//  }

//}
