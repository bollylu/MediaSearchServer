using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models;

public abstract class AMediaSearchDataTableJson : IMediaSearchDataTable, IMediaSearchLoggable<AMediaSearchDataTableJson> {

  public const int TIMEOUT_IN_MS = 5000;
  public const string JSON_HEADER = "header";
  public const string JSON_CONTENT = "medias";

  public const string HEADER_FILE_EXTENSION = ".header.json";
  public const string RECORD_FILE_EXTENSION = ".record.json";

  public IMediaSearchLogger<AMediaSearchDataTableJson> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<AMediaSearchDataTableJson>();

  #region --- Public properties ------------------------------------------------------------------------------
  public string TablePath { get; init; } = "";
  public string TableName { get; init; } = "";
  public string TableFullName => Path.Join(TablePath, TableName);

  public IMSTableHeader<IMedia> Header { get; } = new TMSTableHeader();
  public string HeaderFilename => $"{TableName}{HEADER_FILE_EXTENSION}";
  public string HeaderFullFilename => Path.Join(TablePath, TableName, $"{TableName}{HEADER_FILE_EXTENSION}");

  public bool IsDirty { get; set; } = false;
  #endregion --- Public properties ---------------------------------------------------------------------------

  #region --- Internal data storage --------------------------------------------
  protected readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  protected bool _IsOpened = false;
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaSearchDataTableJson(string tablePath, string name) {
    TablePath = tablePath;
    TableName = name;
    Header.Name = name;
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  protected AMediaSearchDataTableJson(string tablePath, string name, IMediaSource mediaSource) {
    TablePath = tablePath;
    TableName = name;
    Header.Name = name;
    Header.MediaSource = mediaSource;
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  protected AMediaSearchDataTableJson(AMediaSearchDataTableJson table) {
    TablePath = table.TablePath;
    TableName = table.TableName;
    Header.MediaSource = table.Header.MediaSource;
    Header.Name = table.Header.Name;
    Header.Description = table.Header.Description;

    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  public virtual void Dispose() {
    Close();
    _LockData?.Dispose();
  }

  public virtual async ValueTask DisposeAsync() {
    await CloseAsync(CancellationToken.None);
    _LockData?.Dispose();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public virtual string ToString(int indent) {
    try {
      _LockData.EnterReadLock();
      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendIndent($"- {nameof(Header)}", indent)
            .AppendIndent(Header?.ToString() ?? "", indent + 2)
            .AppendIndent($"- {nameof(TablePath)} : {TablePath.WithQuotes()}", indent)
            .AppendIndent($"- {nameof(TableName)} : {TableName.WithQuotes()}", indent)
            .AppendIndent($"- {nameof(TableFullName)} : {TableFullName.WithQuotes()}", indent)
            .AppendIndent($"- {nameof(AutoSave)} : {AutoSave}", indent);
      return RetVal.ToString();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  #region --- I/O --------------------------------------------
  public const int IO_TIMEOUT_IN_MS = 5000;

  #region --- Load --------------------------------------------
  public abstract bool Load();

  public abstract Task<bool> LoadAsync(CancellationToken token);
  #endregion --- Load --------------------------------------------

  #region --- Save --------------------------------------------
  public bool AutoSave { get; set; } = false;

  public abstract bool Save();

  public abstract Task<bool> SaveAsync();
  public abstract Task<bool> SaveAsync(CancellationToken token);
  #endregion --- Save --------------------------------------------

  public bool OpenOrCreate() {
    if (_IsOpened) {
      return true;
    }
    try {

      Logger.IfDebugMessageExBox("Opening json datatable", TableFullName);
      if (Exists()) {
        _IsOpened = true;
        return true;
      }

      try {
        Create();
        _IsOpened = true;
        ClearDirty();
        return true;
      } catch (Exception ex) {
        _IsOpened = false;
        Logger.LogErrorBox($"Unable to create {TableFullName}", ex);
        return false;
      }

    } finally {
      Logger.IfDebugMessageExBox("Json datatable status after open", this);
    }


  }

  public void Close() {

    try {
      Logger.IfDebugMessageExBox("Closing json database", TableFullName);
      if (_IsOpened && IsDirty) {
        Save();
      }
      _IsOpened = false;
      ClearDirty();
    } finally {
      Logger.IfDebugMessageExBox("Json database status", this);
    }

  }

  public async Task CloseAsync(CancellationToken token) {
    try {
      Logger.IfDebugMessageExBox("Closing json database async", TableFullName);
      if (_IsOpened && IsDirty) {
        await SaveAsync(token).ConfigureAwait(false);
      }
      _IsOpened = false;
      ClearDirty();
    } finally {
      Logger.IfDebugMessageExBox("Json database status", this);
    }

  }

  public bool Create() {
    if (Directory.Exists(TableFullName)) {
      return true;
    }

    using (TChrono Chrono = new()) {
      Chrono.Start();
      try {
        try {
          Logger.LogDebug("DataTable directory creation...");
          Directory.CreateDirectory(TableFullName);
          Logger.LogDebug("DataTable header creation ...");
          string RawHeader = ((IJson)Header).ToJson();
          File.WriteAllText(HeaderFullFilename, RawHeader);
          return true;
        } catch (Exception ex) {
          Logger.LogErrorBox($"Unable to create database {TableFullName}", ex);
          return false;
        }
      } finally {
        Chrono.Stop();
        Logger.LogDebug($"Database {TableFullName.WithQuotes()} created in {Chrono.ElapsedTime.DisplayTime()}");
      }
    }

  }

  public void Remove() {
    try {
      Logger.IfDebugMessageExBox("Removing json DataTable", TableFullName);
      if (!Exists()) {
        Logger.LogWarningBox("Unable to remove DataTable", this);
        return;
      }
      try {
        Directory.Delete(TableFullName, true);
        return;
      } catch (Exception ex) {
        Logger.LogErrorBox($"Unable to remove DataTable {TableFullName}", ex);
        throw;
      }
    } finally {
      Logger.IfDebugMessageExBox("Json DataTable status", $"DataTable exists : {Exists()}");
    }

  }

  public bool Exists() {
    if (string.IsNullOrEmpty(TableFullName)) {
      Logger.LogWarning($"Unable to test for existence : Missing {nameof(TableFullName)}");
      return false;
    }
    return Directory.Exists(TableFullName);
  }
  #endregion --- I/O --------------------------------------------

  #region --- Dirty indicator --------------------------------------------
  public virtual void SetDirty() {
    IsDirty = true;
    Header.LastUpdate = DateTime.Now;
  }

  public virtual void ClearDirty() {
    IsDirty = false;
  }
  #endregion --- Dirty indicator --------------------------------------------
}
