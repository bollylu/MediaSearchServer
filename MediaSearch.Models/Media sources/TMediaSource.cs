namespace MediaSearch.Models;

public class TMediaSource : IMediaSource {
  public Type? MediaType { get; protected set; }

  public string RootStorage { get; set; } = "";

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSource() { }

  public TMediaSource(string rootStorage) {
    RootStorage = rootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder();
    RetVal.AppendIndent($"- {nameof(RootStorage)} : {RootStorage.WithQuotes()}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}

public class TMediaSource<RECORD> : TMediaSource, IMediaSource<RECORD> where RECORD : IID {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSource() : base() {
    MediaType = typeof(RECORD);
  }

  public TMediaSource(string rootStorage) : base(rootStorage) {
    MediaType = typeof(RECORD);
  }

  public TMediaSource(IMediaSource<RECORD> source) {
    MediaType = typeof(RECORD);
    RootStorage = source.RootStorage;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(MediaType)} : {MediaType}", indent);
    return RetVal.ToString();
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
