namespace MediaSearch.Database;

public abstract partial class AMSDatabase {

  public abstract bool Create();
  public abstract bool Create(string schema);
  public abstract bool Remove();
  public abstract bool Exists();
  public abstract bool Reindex(IMSTable table);
  public abstract bool DbCheck();
  public abstract void Dispose();

}
