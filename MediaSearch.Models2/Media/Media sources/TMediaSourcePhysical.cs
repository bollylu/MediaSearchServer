using System.Drawing;

namespace MediaSearch.Models;
public class TMediaSourcePhysical : AMediaSource, IMediaSourcePhysical {
  public string StoragePlace { get; set; } = string.Empty;
  public EStorageType StorageType { get; set; }

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMediaSourcePhysical() : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourcePhysical>();
  }
  public TMediaSourcePhysical(IMediaSourcePhysical mediaSource) : base(mediaSource) {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourcePhysical>();
    StoragePlace = mediaSource.StoragePlace;
    StorageType = mediaSource.StorageType;
  }

  protected TMediaSourcePhysical(string storagePlace) : base() {
    Logger = GlobalSettings.LoggerPool.GetLogger<TMediaSourcePhysical>();
    StoragePlace = storagePlace;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public static TMediaSourceVirtual Empty { get { return new TMediaSourceVirtual(); } }

  #region --- Converters -------------------------------------------------------------------------------------
  public override string ToString(int indent) {
    StringBuilder RetVal = new StringBuilder(base.ToString(indent));
    RetVal.AppendIndent($"- {nameof(StoragePlace)} : {StoragePlace.WithQuotes()}", indent);
    RetVal.AppendIndent($"- {nameof(StorageType)} : {StorageType}", indent);
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------
}
