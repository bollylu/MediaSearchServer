namespace MediaSearch.Database;

public class TMSTableHeader<T> : IMSTableHeader<T>, IJson<TMSTableHeader<T>> where T : IID<string> {
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public DateTime LastUpdate { get; set; } = DateTime.MinValue;

  public Type TableType => typeof(T);

  public IMediaSource<IMedia> MediaSource { get; set; } = new TMediaSource<IMedia>();

  public TMSIndex<T, string, string> IndexByName { get; } = new ();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTableHeader() { }
  public TMSTableHeader(IMSTableHeader<T> header) {
    Name = header.Name;
    Description = header.Description;
    LastUpdate = DateTime.Now;
    foreach (var IndexItem in header.IndexByName.IndexedValues) {
      IndexByName.IndexedValues.Add(IndexItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(MediaSource)}", indent)
          .AppendIndent(MediaSource, indent + 2)
          .AppendIndent($"- {nameof(Name)} : {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} : {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(LastUpdate)} : {LastUpdate.ToYMDHMS()}", indent)
          .AppendIndent($"- {nameof(IndexByName)} : {IndexByName.IndexedValues.Count()} item(s)", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters ----------------------------------------------------------------------------------
}
