namespace MediaSearch.Storage;
public abstract class AStorage : ALoggable, IStorage {
  protected AStorage() { }

  //public string PhysicalDataPath { get; set; } = "";

  public abstract ValueTask<bool> Exists();
  public abstract ValueTask<bool> Create();
  public abstract ValueTask<bool> Remove();
  public abstract ValueTask<bool> Any();
  public abstract ValueTask<bool> IsEmpty();

  public abstract Task Clear();
}
