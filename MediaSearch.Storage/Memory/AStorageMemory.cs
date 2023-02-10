namespace MediaSearch.Storage;
public abstract class AStorageMemory : AStorage {

  protected List<IMedia> Medias = new List<IMedia>();

  protected readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

  #region --- IStorage --------------------------------------------
  public override bool Exists() {
    return true;
  }

  public override bool Create() {
    Medias.Clear();
    return true;
  }

  public override bool Remove() {
    Medias.Clear();
    return true;
  }
  #endregion --- IStorage --------------------------------------------

  public string PhysicalDataPath { get; init; } = "";
}
