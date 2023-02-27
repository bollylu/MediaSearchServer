namespace MediaSearch.Storage;
public class TStorageJson : AStorage {
  #region --- IStorage --------------------------------------------
  public override ValueTask<bool> Exists() {
    return ValueTask.FromResult(true);
  }

  public override ValueTask<bool> Create() {
    return ValueTask.FromResult(true);
  }

  public override ValueTask<bool> Remove() {
    return ValueTask.FromResult(true);
  }

  public override ValueTask<bool> Any() {
    throw new NotImplementedException();
  }

  public override ValueTask<bool> IsEmpty() {
    throw new NotImplementedException();
  }

  public override Task Clear() {
    throw new NotImplementedException();
  }
  #endregion --- IStorage --------------------------------------------
}
