namespace MediaSearch.Models;

public abstract class AMediaSource<RECORD> : IMediaSource, IMediaSource<RECORD> where RECORD : class, IRecord {

  public Type MediaType => typeof(RECORD);

  public string RootStorage { get; set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaSource() {
  }

  protected AMediaSource(string rootStorage) {
    RootStorage = rootStorage;
  }

  protected AMediaSource(IMediaSource source) {
    RootStorage = source.RootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(MediaType)} : {MediaType}", indent);
    RetVal.AppendIndent($"- {nameof(RootStorage)} : {RootStorage.WithQuotes()}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
