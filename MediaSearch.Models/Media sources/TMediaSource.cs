namespace MediaSearch.Models;
public class TMediaSource<T> : IMediaSource<T> where T : IMedia {

  #region --- Public properties ------------------------------------------------------------------------------
  public Type MediaType => typeof(T);
  public string RootStorage { get; set; } = "";
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSource() { }
  
  public TMediaSource(string rootStorage) {
    RootStorage = rootStorage;
  }

  public TMediaSource(IMediaSource<T> source) {
    RootStorage = source.RootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(MediaType)} : {MediaType}", indent)
          .AppendIndent($"- {nameof(RootStorage)} : {RootStorage.WithQuotes()}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
