namespace MediaSearch.Models;
public abstract class ADataModel : ALoggable {

  [JsonIgnore]
  public override ILogger Logger {
    get {
      return base.Logger;
    }

    set {
      base.Logger = value;
    }
  }

}
