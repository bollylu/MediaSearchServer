namespace MediaSearch.Storage;
public abstract class AStorage : ALoggable, IStorage {
  protected AStorage() { }

  public string PhysicalDataPath { get; set; } = "";

  public abstract bool Exists();
  public abstract bool Create();
  public abstract bool Remove();
}
