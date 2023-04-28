namespace MediaSearch.Models;
public class TMediaSource : AMediaSource {
  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSource() : base() { }
  public TMediaSource(IMediaSource mediaSource) : base(mediaSource) {
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static TMediaSource Empty { get { return new TMediaSource(); } }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}

