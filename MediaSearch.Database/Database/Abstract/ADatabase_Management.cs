namespace MediaSearch.Database;

public abstract partial class ADatabase {

  public abstract bool Create();
  public abstract bool Create(string schema);
  public abstract bool Remove();
  public abstract bool Exists();
  public virtual bool DbCheck() {
    return Schema.GetAllTables().All(t => TableCheck(t));
  }
  public abstract void Dispose();

}
