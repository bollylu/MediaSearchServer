using System.Runtime.CompilerServices;

using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models;

public class TMSTableMovie : IMSTable<IMovie>, IMediaSearchLoggable<TMSTableMovie> {

  public IMediaSearchLogger<TMSTableMovie> Logger { get; } = GlobalSettings.LoggerPool.GetLogger<TMSTableMovie>();

  public IMSTableHeader<IMovie> Header { get; private set; } = new TMSTableHeader<IMovie>();

  #region --- Internal data storage --------------------------------------------
  protected readonly ReaderWriterLockSlim _LockData = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);

  protected bool _IsOpened = false;
  #endregion --- Internal data storage --------------------------------------------

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TMSTableMovie() {
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  public TMSTableMovie(TMSTableMovie table)  {
    Header = new TMSTableHeader<IMovie>(table.Header);
    Logger.SeverityLimit = ESeverity.DebugEx;
    SetDirty();
  }

  public void Dispose() {
  }

  public ValueTask DisposeAsync() {
    return ValueTask.CompletedTask;
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  #region --- Converters -------------------------------------------------------------------------------------
  public string ToString(int indent) {

    StringBuilder RetVal = new StringBuilder();

    try {
      _LockData.EnterReadLock();
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

  #region --- Content information --------------------------------------------
  public bool Any() {
    try {
      _LockData.EnterReadLock();
      return Header.IndexByName.Any();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public bool IsEmpty() {
    try {
      _LockData.EnterReadLock();
      return Header.IndexByName.IsEmpty();
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public int Count() {
    try {
      _LockData.EnterReadLock();
      return Header.IndexByName.Count;
    } finally {
      _LockData.ExitReadLock();
    }
  }
  #endregion --- Content information --------------------------------------------

  #region --- Content management --------------------------------------------

  public void Add(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    item.SetDirty();

    try {
      _LockData.EnterUpgradeableReadLock();
      if (Header.IndexByName.ContainsValue(item.Id)) {
        Logger.LogWarningBox("Unable to add item : item already exists", item);
        return;
      }

      Save(item);
      try {
        _LockData.EnterWriteLock();
        UpdateHeader(item);
      } finally {
        _LockData.ExitWriteLock();
      }
    } finally {
      _LockData.ExitUpgradeableReadLock();
    }
  }

  public async Task AddAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterUpgradeableReadLock();
      IMedia? FoundIt = _Items.Find(x => x.Id == item.Id);
      if (FoundIt is null) {
        _LockData.EnterWriteLock();
        try {
          _Items.Add(item);
          SetDirty();
        } finally {
          _LockData.ExitWriteLock();
        }
      } else {
        Logger.LogErrorBox("Duplicate ID : Original", FoundIt);
        Logger.LogErrorBox("Duplicate ID : Second", item);
      }
    } finally {
      _LockData.ExitUpgradeableReadLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }

  public void AddOrUpdate(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        _Items.Add(item);
      } else {
        _Items[ItemIndex] = item;
      }

      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      Save();
    }
  }

  public async Task AddOrUpdateAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        _Items.Add(item);
      } else {
        _Items[ItemIndex] = item;
      }
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }

  public void Update(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to update is not found : {item.Id}");
      }
      _Items[ItemIndex] = item;
      _Items.Add(item);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
    if (AutoSave) {
      Save();
    }
  }

  public async Task UpdateAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMedia item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMedia item to update is not found : {item.Id}");
      }
      _Items[ItemIndex] = item;
      _Items.Add(item);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }

  public void Delete(IMovie item) {
    if (item is null) {
      throw new ApplicationException("IMovie item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMIMovieedia item to delete is not found : {item.Id}");
      }
      _Items.RemoveAt(ItemIndex);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      Save();
    }
  }

  public async Task DeleteAsync(IMovie item, CancellationToken token) {
    if (item is null) {
      throw new ApplicationException("IMovie item is missing");
    }

    try {
      _LockData.EnterWriteLock();
      int ItemIndex = _Items.FindIndex(x => x.Id == item.Id);
      if (ItemIndex < 0) {
        throw new ApplicationException($"IMovie item to delete is not found : {item.Id}");
      }
      _Items.RemoveAt(ItemIndex);
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }

    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }
  
  public void Clear() {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
    if (AutoSave) {
      Save();
    }
  }

  public async Task ClearAsync(CancellationToken token) {
    try {
      _LockData.EnterWriteLock();
      _Items.Clear();
      SetDirty();
    } finally {
      _LockData.ExitWriteLock();
    }
    if (AutoSave) {
      await SaveAsync(token).ConfigureAwait(false);
    }
  }

  public IMovie? Get(string id) {
    if (string.IsNullOrWhiteSpace(id)) {
      throw new ArgumentNullException(nameof(id));
    }

    try {
      _LockData.EnterReadLock();
      return _Items.FirstOrDefault(x => x.Id == id);
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public IEnumerable<IMovie> GetAll(int maxRecords = 0) {

    if (IsEmpty()) {
      yield break;
    }

    maxRecords = maxRecords.WithinLimits(0, int.MaxValue);

    try {
      _LockData.EnterReadLock();
      if (maxRecords == 0) {
        foreach (IMovie MediaItem in _Items) {
          yield return MediaItem;
        }
      } else {
        foreach (IMovie MediaItem in _Items.Take(maxRecords)) {
          yield return MediaItem;
        }
      }
    } finally {
      _LockData.ExitReadLock();
    }

  }

  public async IAsyncEnumerable<IMovie> GetAllAsync([EnumeratorCancellation] CancellationToken token) {

    if (IsEmpty()) {
      yield break;
    }

    try {
      _LockData.EnterReadLock();
      await foreach (IMovie MediaItem in _Items.ToAsyncEnumerable().WithCancellation(token)) {
        yield return MediaItem;
      }
    } finally {
      _LockData.ExitReadLock();
    }


  }

  public IEnumerable<IMovie> GetFiltered(IFilter filter, int maxRecords = 0) {

    if (filter is null) {
      yield break;
    }

    if (IsEmpty()) {
      yield break;
    }

    maxRecords = maxRecords.WithinLimits(0, int.MaxValue);

    IList<IMedia> LocalItems;
    try {
      _LockData.EnterReadLock();
      if (maxRecords == 0) {
        LocalItems = new List<IMedia>(_Items.WithFilter(filter).OrderedBy(filter));
      } else {
        LocalItems = new List<IMedia>(_Items.WithFilter(filter).OrderedBy(filter).Take(maxRecords));
      }
    } finally {
      _LockData.ExitReadLock();
    }

    foreach (IMovie MediaItem in LocalItems) {
      yield return MediaItem;
    }

  }

  public async IAsyncEnumerable<IMovie> GetFilteredAsync(TFilter filter, [EnumeratorCancellation] CancellationToken token) {

    if (filter is null) {
      yield break;
    }

    if (IsEmpty()) {
      yield break;
    }

    IList<IMedia> LocalItems;
    try {
      _LockData.EnterReadLock();
      LocalItems = new List<IMedia>(_Items.WithFilter(filter).OrderedBy(filter));
    } finally {
      _LockData.ExitReadLock();
    }

    await foreach (IMovie MediaItem in LocalItems.ToAsyncEnumerable().WithCancellation(token)) {
      yield return MediaItem;
    }
  }

  #endregion --- Content management --------------------------------------------

  #region --- I/O --------------------------------------------

  #region --- Load --------------------------------------------
  public override bool Load() {
    if (!_IsOpened) {
      throw new ApplicationException("Database needs to be opened to load items");
    }

    using (TChrono Chrono = new()) {
      Chrono.Start();
      try {
        bool OldAutoSave = AutoSave;
        AutoSave = false;
        Clear();

        try {
          IEnumerable<string> Records = Directory.EnumerateFiles(TableFullName, $"*{RECORD_FILE_EXTENSION}");
          foreach (string RecordItem in Records.AsParallel()) {
            try {
              string DataSourceContent = File.ReadAllText(RecordItem);
              JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
              JsonElement JsonMovie = JsonContent.RootElement;
              IMovie? Movie = IJson<TMovie>.FromJson(JsonMovie.GetRawText());
              if (Movie is not null) {
                Movie.ClearDirty();
                Add(Movie);
              }
            } catch (Exception ex) {
              Logger.LogErrorBox($"Unable to load data from {TableFullName.WithQuotes()} : {RecordItem}", ex);
              return false;
            }
          }
        } finally {
          AutoSave = OldAutoSave;
        }

        try {
          _LockData.EnterWriteLock();
          ClearDirty();
        } finally {
          _LockData?.ExitWriteLock();
        }
      } finally {
        Chrono.Stop();
        StringBuilder Result = new();
        Result.AppendLine($"{Count()} records");
        Result.AppendLine(Chrono.ElapsedTime.DisplayTime());
        Logger.LogDebugBox("Load database result", Result);
      }
    }
    return true;
  }

  public override async Task<bool> LoadAsync(CancellationToken token) {
    if (!_IsOpened) {
      throw new ApplicationException("Database needs to be opened to load items");
    }

    using (TChrono Chrono = new()) {
      Chrono.Start();
      try {
        bool OldAutoSave = AutoSave;
        AutoSave = false;
        Clear();

        try {
          IEnumerable<string> Records = Directory.EnumerateFiles(TableFullName, $"*{RECORD_FILE_EXTENSION}");
          foreach (string RecordItem in Records.AsParallel()) {
            if (token.IsCancellationRequested) {
              return false;
            }
            try {
              string DataSourceContent = await File.ReadAllTextAsync(RecordItem, token).ConfigureAwait(false);
              JsonDocument JsonContent = JsonDocument.Parse(DataSourceContent);
              JsonElement JsonMovie = JsonContent.RootElement;
              IMovie? Movie = IJson<TMovie>.FromJson(JsonMovie.GetRawText());
              if (Movie is not null) {
                Movie.ClearDirty();
                Add(Movie);
              }
            } catch (Exception ex) {
              Logger.LogErrorBox($"Unable to load data from {TableFullName.WithQuotes()} : {RecordItem}", ex);
              return false;
            }
          }
        } finally {
          AutoSave = OldAutoSave;
        }

        try {
          _LockData.EnterWriteLock();
          ClearDirty();
        } finally {
          _LockData?.ExitWriteLock();
        }
      } finally {
        Chrono.Stop();
        StringBuilder Result = new();
        Result.AppendLine($"{Count()} records");
        Result.AppendLine(Chrono.ElapsedTime.DisplayTime());
        Logger.LogDebugBox("Load database result", Result);
      }
    }

    return true;
  }
  #endregion --- Load --------------------------------------------

  #region --- Save --------------------------------------------

  public virtual bool Save(IMovie movie) {

    using (TChrono Chrono = new()) {
      Chrono.Start();

      try {
        Logger.IfDebugMessageEx($"Saving movie to {movie.Id}");

        

        try {
          _LockData.EnterWriteLock();
          ClearDirty();
        } finally {
          _LockData.ExitWriteLock();
        }

      } finally {
        Chrono.Stop();
        Logger.IfDebugMessageExBox($"Database content saved to {TableFullName}", $"{Count()} records in {Chrono.ElapsedTime.TotalMilliseconds} ms");
      }

      return true;
    }
  }

  public override bool Save() {

    using (TChrono Chrono = new()) {
      Chrono.Start();

      try {
        Logger.IfDebugMessageExBox($"Saving database content to {TableFullName}", $"{Count()} records");
        if (!_IsOpened) {
          return false;
        }
        if (!IsDirty) {
          return true;
        }

        foreach (IMovie RecordItem in GetAllDirty().AsParallel()) {
          string RecordName = $"{Path.Combine(TableFullName, RecordItem.Id)}.record.json";
          try {
            using (FileStream OutputStream = new FileStream(RecordName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)) {
              using (Utf8JsonWriter Writer = new Utf8JsonWriter(OutputStream, new JsonWriterOptions() { Indented = true, Encoder = IJson.DefaultJsonSerializerOptions.Encoder })) {
                JsonSerializer.Serialize(Writer, (IMovie)RecordItem, IJson.DefaultJsonSerializerOptions);
                Writer.Flush();
              }
            }
          } catch (Exception ex) {
            Logger.LogErrorBox($"Unable to save data to {TableFullName.WithQuotes()} : {RecordName}", ex);
            return false;
          }
        }

        try {
          _LockData.EnterWriteLock();
          ClearDirty();
        } finally {
          _LockData.ExitWriteLock();
        }

      } finally {
        Chrono.Stop();
        Logger.IfDebugMessageExBox($"Database content saved to {TableFullName}", $"{Count()} records in {Chrono.ElapsedTime.TotalMilliseconds} ms");
      }

      return true;
    }
  }

  public override async Task<bool> SaveAsync() {
    return await SaveAsync(CancellationToken.None);
  }

  public override async Task<bool> SaveAsync(CancellationToken token) {

    using (TChrono Chrono = new()) {
      Chrono.Start();
      try {
        Logger.IfDebugMessageExBox($"Saving database content to {TableFullName}", $"{_Items.Count} records");

        if (!_IsOpened) {
          return false;
        }
        if (!IsDirty) {
          return true;
        }

        try {

          await foreach (IMovie MovieItem in GetAllDirtyAsync(token).ConfigureAwait(false)) {
            if (token.IsCancellationRequested) {
              return false;
            }
            string RecordName = $"{Path.Combine(TableFullName, MovieItem.Id)}.json";
            using (MemoryStream OutputStream = new MemoryStream()) {
              await JsonSerializer.SerializeAsync(OutputStream, MovieItem, IJson.DefaultJsonSerializerOptions, token).ConfigureAwait(false);
              await File.WriteAllBytesAsync(RecordName, OutputStream.ToArray(), token).ConfigureAwait(false);
            }
          }
          ClearDirty();
          return true;

        } catch (Exception ex) {
          Logger.LogErrorBox("Unable to commit changes to storage", ex);
          return false;

        }
      } finally {
        Chrono.Stop();
        Logger.IfDebugMessageExBox($"Database content saved to {TableFullName}", $"{Count()} records in {Chrono.ElapsedTime.TotalMilliseconds} ms");
      }
    }
  }
  #endregion --- Save --------------------------------------------
  
  #endregion --- I/O --------------------------------------------

  #region --- Dirty indicator --------------------------------------------
  public override void SetDirty() {
    IsDirty = true;
    Header.LastUpdate = DateTime.Now;
  }

  public override void ClearDirty() {
    IsDirty = false;
  }

  public IEnumerable<IMovie> GetAllDirty() {
    if (IsEmpty()) {
      yield break;
    }

    try {
      _LockData.EnterReadLock();
      foreach (IMovie MovieItem in _Items.Where(x => x.IsDirty)) {
        yield return MovieItem;
      }
    } finally {
      _LockData.ExitReadLock();
    }
  }

  public async IAsyncEnumerable<IMovie> GetAllDirtyAsync([EnumeratorCancellation] CancellationToken token) {
    if (IsEmpty()) {
      yield break;
    }

    try {
      _LockData.EnterReadLock();
      await foreach (IMovie MovieItem in _Items.Where(x => x.IsDirty).ToAsyncEnumerable()) {
        if (token.IsCancellationRequested) {
          yield break;
        }
        yield return MovieItem;
      }
    } finally {
      if (_LockData.WaitingReadCount > 0) {
        _LockData.ExitReadLock();
      }
    }
  }
  #endregion --- Dirty indicator --------------------------------------------
}
