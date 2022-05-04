namespace MediaSearch.Models;

public abstract class AMediaSearchDataTableMemory : IMediaSearchDataTable, IMediaSearchLoggable<AMediaSearchDataTableMemory> {

  public IMediaSearchLogger<AMediaSearchDataTableMemory> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<AMediaSearchDataTableMemory>();

  public IMSTableHeader Header { get; } = new TMSTableHeader();

  public bool IsDirty { get; set; } = false;

  #region --- Internal data storage --------------------------------------------

  protected readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  protected AMediaSearchDataTableMemory() { }

  protected AMediaSearchDataTableMemory(AMediaSearchDataTableMemory table) {
    Header = new TMSTableHeader(table.Header);
  }

  public void Dispose() {
    Close();
    _LockData?.Dispose();
  }

  public async ValueTask DisposeAsync() {
    await CloseAsync(CancellationToken.None);
    _LockData?.Dispose();
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {
    try {
      _LockData.EnterReadLock();

      StringBuilder RetVal = new StringBuilder();
      RetVal.AppendIndent($"- {nameof(Header)}", indent)
            .AppendIndent(Header?.ToString() ?? "", indent + 2);
      return RetVal.ToString();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public override string ToString() {
    return ToString(0);
  }
  #endregion --- Converters -------------------------------------------------------------------------------------

  public bool Exists() {
    return true;
  }

  public bool Create() {
    return true;
  }

  public void Remove() {
    return;
  }

  public bool OpenOrCreate() {
    return true;
  }

  public void Close() {
  }

  public Task CloseAsync(CancellationToken token) {
    return Task.CompletedTask;
  }

  public bool Load() {
    return true;
  }

  public Task<bool> LoadAsync(CancellationToken token) {
    return Task.FromResult(true);
  }

  public bool AutoSave { get; set; } = false;

  public bool Save() {
    return true;
  }

  public async Task<bool> SaveAsync() {
    return await SaveAsync(CancellationToken.None);
  }
  public Task<bool> SaveAsync(CancellationToken token) {
    return Task.FromResult(true);
  }

  #region --- Dirty indicator --------------------------------------------
  private void SetDirty() {
    IsDirty = true;
    Header.LastUpdate = DateTime.Now;
  }

  private void ClearDirty() {
    IsDirty = false;
  }
  #endregion --- Dirty indicator --------------------------------------------
}
