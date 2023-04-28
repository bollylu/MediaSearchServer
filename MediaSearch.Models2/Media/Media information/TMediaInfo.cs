namespace MediaSearch.Models;
public class TMediaInfo : AMediaInfo {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfo() : base() { }

  public TMediaInfo(IMediaInfo mediaInfo) : base(mediaInfo) {
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public static TMediaInfo Empty { get { return new TMediaInfo(); } }
}
