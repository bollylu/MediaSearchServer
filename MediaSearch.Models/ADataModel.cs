namespace MediaSearch.Models;
public abstract class ADataModel : IMediaSearchLoggable {

  [JsonIgnore]
  public IMediaSearchLogger Logger { get; set; } = AMediaSearchLogger.Create(GlobalSettings.GlobalLogger);

}
