namespace MediaSearch.Storage;
public class TStorageJson : AStorage {
  #region --- IStorage --------------------------------------------
  public override bool Exists() {
    return true;
  }

  public override bool Create() {
    return true;
  }

  public override bool Remove() {
    return true;
  }
  #endregion --- IStorage --------------------------------------------
}
