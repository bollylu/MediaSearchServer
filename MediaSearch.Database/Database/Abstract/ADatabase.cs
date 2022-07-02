namespace MediaSearch.Database;
public abstract partial class ADatabase : IDatabase, ILoggable {

  public abstract ILogger Logger { get; set; }

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";

  public abstract string ToString(int indent);

  public bool IsOpened { get; protected set; } = false;

  public virtual string Dump() {
    StringBuilder RetVal = new();
    RetVal.AppendLine($"{nameof(Name)} = {Name.WithQuotes()}");
    RetVal.AppendLine($"{nameof(Description)} = {Description.WithQuotes()}");


    return RetVal.ToString();
  }

}
