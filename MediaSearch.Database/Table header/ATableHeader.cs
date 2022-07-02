namespace MediaSearch.Database;

public abstract class ATableHeader<RECORD> : ITableHeader<RECORD> where RECORD : IRecord {

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public DateTime LastUpdate { get; set; } = DateTime.MinValue;
  public IMediaSource? MediaSource { get; protected set; }

  public Type TableType => typeof(RECORD);

  public TIndex<RECORD, string, string> IndexByName { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected ATableHeader() {
    LastUpdate = DateTime.Now;
    MediaSource = TMediaSource.Create(typeof(RECORD).Name);
  }

  protected ATableHeader(ITableHeader<RECORD> header) {
    Name = header.Name;
    Description = header.Description;
    LastUpdate = DateTime.Now;
    MediaSource = header.MediaSource;
    foreach (var IndexItem in header.IndexByName.IndexedValues) {
      IndexByName.IndexedValues.Add(IndexItem);
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(MediaSource)}", indent)
          .AppendIndent(MediaSource?.ToString(indent) ?? "(null)", indent + 2)
          .AppendIndent($"- {nameof(Name)} : {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} : {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(LastUpdate)} : {LastUpdate.ToYMDHMS()}", indent);
    RetVal.AppendIndent($"- {nameof(IndexByName)} : {IndexByName.IndexedValues.Count} item(s)", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters ----------------------------------------------------------------------------------

  public bool SetMediaSource(IMediaSource mediaSource) {
    MediaSource = mediaSource;
    return true;
  }
}

