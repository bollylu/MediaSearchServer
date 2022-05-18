﻿namespace MediaSearch.Database;
public abstract partial class AMSDatabase : IMSDatabase, IMediaSearchLoggable {

  public abstract IMediaSearchLogger Logger { get; }

  public string Name { get; set; } = "";
  public string Description { get; set; } = "";
  public abstract string DatabaseFullName { get; }

  public abstract string ToString(int indent);

  public bool IsOpened { get; protected set; } = false;

  public virtual string Dump() {
    StringBuilder RetVal = new();
    RetVal.AppendLine($"{nameof(Name)} = {Name}");
    RetVal.AppendLine($"{nameof(Description)} = {Description}");
    RetVal.AppendLine($"{nameof(DatabaseFullName)} = {DatabaseFullName}");
    
    return RetVal.ToString();
  }

}