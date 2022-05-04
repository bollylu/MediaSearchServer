namespace MediaSearch.Database;

public class TMSDatabaseJson : IMSDatabase, IMediaSearchLoggable<TMSDatabaseJson> {

  public IMediaSearchLogger<TMSDatabaseJson> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSDatabaseJson>();

  public string Name { get; set; } = "";

  public string Description { get; set; } = "";

  public string RootPath { get; set; } = ".\\";

  public string DatabaseFullName {
    get {
      return Path.Join(RootPath, Name);
    }
  }

  public List<IMSTable> Tables { get; } = new List<IMSTable>();

  public string GetSchema() {
    return "";
  }

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    StringBuilder RetVal = new();
    RetVal.AppendIndent($"- {nameof(Name)} = {Name.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(Description)} = {Description.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(RootPath)} = {RootPath.WithQuotes()}", indent)
          .AppendIndent($"- {nameof(DatabaseFullName)} = {DatabaseFullName.WithQuotes()}", indent);
    if (Tables.Any()) {
      RetVal.AppendIndent($"- {nameof(Tables)}", indent);
      foreach (IMSTable TableItem in Tables) {
        RetVal.AppendIndent(TableItem.ToString(indent), indent + 2);
      }
    }else {
      RetVal.AppendIndent("- No table available", indent);
    }
    return RetVal.ToString();
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public bool Create() {
    using (TChrono Chrono = new()) {
      try {
        Chrono.Start();

        if (!Directory.Exists(DatabaseFullName)) {
          Directory.CreateDirectory(DatabaseFullName);
        }

        Chrono.Stop();
        Logger.LogDebug("Database creation successful", $"{Chrono.ElapsedTime.DisplayTime()}\n{this}");

        return true;
      } catch (Exception ex) {
        Logger.LogErrorBox($"Unable to create database {Name}", ex);
        return false;
      }
    }
  }

  public bool Create(string schema) {
    if (string.IsNullOrEmpty(schema)) {
      Logger.LogError($"Unable to create database {Name.WithQuotes()} via schema : schema is null or invalid");
      return false;
    }

    using (TChrono Chrono = new()) {
      try {
        Chrono.Start();

        if (!Directory.Exists(DatabaseFullName)) {
          Directory.CreateDirectory(DatabaseFullName);
        }

        Chrono.Stop();
        Logger.LogDebug("Database creation successful", $"{Chrono.ElapsedTime.DisplayTime()}\n{this}");

        return true;
      } catch (Exception ex) {
        Logger.LogErrorBox($"Unable to create database {Name}", ex);
        return false;
      }
    }
  }

  public bool Remove() {
    using (TChrono Chrono = new()) {
      try {
        Chrono.Start();

        if (Directory.Exists(DatabaseFullName)) {
          Directory.Delete(DatabaseFullName, true);
        }

        Chrono.Stop();
        Logger.LogDebug("Database removal successful", $"{Chrono.ElapsedTime.DisplayTime()}\n{this}");

        return true;
      } catch (Exception ex) {
        Logger.LogErrorBox($"Unable to remove database {Name}", ex);
        return false;
      }
    }
  }

  public bool Exists() {
    return Directory.Exists(DatabaseFullName);
  }

  public bool Reindex() {
    return true;
  }

  public bool DbCheck() {
    return true;
  }

  public bool AddTable(IMSTable table) {
    Tables.Add(table);
    return true;
  }

  public bool Save(IMSTable table) {
    throw new NotImplementedException();
  }

  public bool Save(IMSTable table, IMSRecord record) {
    throw new NotImplementedException();
  }

  public RECORD Get<RECORD>(string table, string key)
    where RECORD : IMSRecord {
    throw new NotImplementedException();
  }
}
