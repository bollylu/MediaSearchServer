namespace MediaSearch.Models;
public class TMSTableHeader<T> : IMSTableHeader<T>, IJson<TMSTableHeader<T>> where T : IMedia {
  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public DateTime LastUpdate { get; set; } = DateTime.MinValue;

  public Type DbType => typeof(T);

  public IMediaSource<T> MediaSource { get; set; } = new TMediaSource<T>();

  public Dictionary<string, string> IndexByName { get; } = new();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTableHeader() { }
  public TMSTableHeader(IMSTableHeader<T> header) {
    Name = header.Name;
    Description = header.Description;
    LastUpdate = DateTime.Now;
    MediaSource = new TMediaSource<T>(header.MediaSource);
    foreach(KeyValuePair<string, string> kvp in header.IndexByName) {
      IndexByName.Add(kvp.Key, kvp.Value);
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
          .AppendIndent($"- {nameof(IndexByName)} : {IndexByName.Count} item(s)", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters ----------------------------------------------------------------------------------
}
