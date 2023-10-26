namespace MediaSearch.Models;
public class TMediaInfo : AMediaInfo {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaInfo() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaInfo>();
  }

  public TMediaInfo(IMediaInfo mediaInfo) : base(mediaInfo) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaInfo>();
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

  public static TMediaInfo Empty { get { return new(); } }
}
