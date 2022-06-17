namespace MediaSearch.Database;

public class TMSTableHeader : AMSTableHeader<IMSRecord> {
  public static IMSTableHeader? Create(string tableName, Type tableType) {
    return tableType.Name switch {
      nameof(TMSTableHeaderMovie) => new TMSTableHeaderMovie(),
      _ => throw new ApplicationException($"Unable to create TableHeader of type {tableType.Name}"),
    };
  }
}

public class TMSTableHeaderMovie : AMSTableHeader<IMovie> {
  public TMSTableHeaderMovie() { }

}
