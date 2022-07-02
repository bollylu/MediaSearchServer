namespace MediaSearch.Database;

public partial class TDatabaseJson {

  public override bool Create() {
    using (TChrono Chrono = new()) {

      return Chrono.ExecuteFunction(() => {
        try {
          if (!Directory.Exists(DatabaseFullName)) {
            Directory.CreateDirectory(DatabaseFullName);
            SaveSchema();
          }

          Chrono.Stop();
          Logger.LogDebugBox($"Database creation successful : {Chrono.ElapsedTime.DisplayTime()}", this);

          return true;
        } catch (Exception ex) {
          Logger.LogErrorBox($"Unable to create database {Name}", ex);
          return false;
        }
      });

    }
  }

  public override bool Create(string schema) {
    if (string.IsNullOrEmpty(schema)) {
      Logger.LogError($"Unable to create database {Name.WithQuotes()} via schema : schema is null or invalid");
      return false;
    }

    using (TChrono Chrono = new()) {

      return Chrono.ExecuteFunction(() => {
        try {
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
      });
    }
  }

  public override bool Remove() {
    using (TChrono Chrono = new()) {

      return Chrono.ExecuteFunction(() => {
        try {
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
      });
    }
  }

  public override bool Exists() {
    return Directory.Exists(DatabaseFullName);
  }

}
