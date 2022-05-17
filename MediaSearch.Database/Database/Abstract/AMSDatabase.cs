namespace MediaSearch.Database;
public abstract partial class AMSDatabase : IMSDatabase, IMediaSearchLoggable {

  public abstract IMediaSearchLogger Logger { get; }

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public abstract string DatabaseFullName { get; }

  public abstract string ToString(int indent);

  public bool IsOpened { get; protected set; } = false;
  
}
