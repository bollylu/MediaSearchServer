namespace MediaSearch.Database;

public class TMSTableHeader : IMSTableHeader {
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public DateTime LastUpdate { get; set; } = DateTime.MinValue;
  public IMediaSource MediaSource { get; protected set; } = new TMediaSource();

  public virtual Type? TableType { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTableHeader() {
    LastUpdate = DateTime.Now;
  }
  public TMSTableHeader(IMSTableHeader header) {
    Name = header.Name;
    Description = header.Description;
    LastUpdate = DateTime.Now;
    TableType = header.TableType;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(MediaSource)}", indent)
          .AppendIndent(MediaSource, indent + 2)
          .AppendIndent($"- {nameof(Name)} : {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} : {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(LastUpdate)} : {LastUpdate.ToYMDHMS()}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters ----------------------------------------------------------------------------------

  public virtual bool SetMediaSource(IMediaSource mediaSource) {
    MediaSource = mediaSource;
    return true;
  }
}



public class TMSTableHeader<RECORD> : TMSTableHeader, IMSTableHeader<RECORD> where RECORD : IID<string> {

  public override Type TableType => typeof(RECORD);

  public TMSIndex<RECORD, string, string> IndexByName { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTableHeader() {
    MediaSource = new TMediaSource<RECORD>();
  }
  public TMSTableHeader(IMSTableHeader header) : base(header) {
    MediaSource = new TMediaSource<RECORD>();
    if (header is IMSTableHeader<RECORD> Header) {
      foreach (var IndexItem in Header.IndexByName.IndexedValues) {
        IndexByName.IndexedValues.Add(IndexItem);
      }
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(IndexByName)} : {IndexByName.IndexedValues.Count()} item(s)", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters ----------------------------------------------------------------------------------

  public override bool SetMediaSource(IMediaSource mediaSource) {
    if (mediaSource is not IMediaSource<RECORD>) {
      return false;
    }
    MediaSource = mediaSource;
    return true;
  }
}
