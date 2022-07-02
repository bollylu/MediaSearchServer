namespace MediaSearch.Database;

public static class TTableHeader {
  public static ITableHeader? Create(Type tableType) {
    return tableType.Name switch {
      nameof(IMovie) => new TMSTableHeaderMovie(),
      _ => throw new ApplicationException($"Unable to create TableHeader of type {tableType.Name}"),
    };
  }

  public static ITableHeader? Create(string? tableType) {
    return tableType switch {
      nameof(IMovie) => new TMSTableHeaderMovie(),
      _ => throw new ApplicationException($"Unable to create TableHeader of type {tableType}"),
    };
  }

  public static ITableHeader? Create(string tableName, Type tableType) {
    return tableType.Name switch {
      nameof(IMovie) => new TMSTableHeaderMovie() { Name = tableName },
      _ => throw new ApplicationException($"Unable to create TableHeader of type {tableType.Name}"),
    };
  }
}

public class TMSTableHeaderMovie : ATableHeader<IMovie> {
  public TMSTableHeaderMovie() : base() { }

}
