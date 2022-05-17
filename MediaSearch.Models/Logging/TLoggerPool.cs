﻿namespace MediaSearch.Models.Logging;
public class TLoggerPool {

  public const string DEFAULT_LOGGER_NAME = "(default)";
  private List<IMediaSearchLogger> _Items { get; } = new();
  public Type DefaultLoggerType { get; set; } = typeof(TMediaSearchLoggerConsole);

  private readonly object _Lock = new object();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLoggerPool() { }

  public TLoggerPool(IEnumerable<IMediaSearchLogger> Items) {
    _Items.AddRange(Items);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public void AddLogger(IMediaSearchLogger logger) {
    lock (_Lock) {
      if (logger.GetType().IsGenericType) {
        logger.Name = logger.GetType().GetTypeInfo().GenericTypeArguments[0].FullName ?? logger.GetType().GetTypeInfo().GenericTypeArguments[0].Name;
      }
      if (string.IsNullOrWhiteSpace(logger.Name)) {
        logger.Name = logger.GetType().FullName ?? logger.GetType().Name;
      }

      if (_Items.Any(x => x.Name.Equals(logger.Name, StringComparison.InvariantCultureIgnoreCase))) {
        throw new ApplicationException($"Attempt to create two logger with the same name : {logger.Name}");
      }
      _Items.Add(logger);
    }
  }

  public void AddDefaultLogger(IMediaSearchLogger logger) {
    lock (_Lock) {
      logger.Name = DEFAULT_LOGGER_NAME;
      if (_Items.Any(x => x.Name.Equals(logger.Name, StringComparison.InvariantCultureIgnoreCase))) {
        throw new ApplicationException($"Attempt to create two logger with the same name : {logger.Name}");
      }
      _Items.Add(logger);
    }
  }

  public IMediaSearchLogger<T> GetDefaultLogger<T>() {
    lock (_Lock) {
      if (_Items.IsEmpty()) {
        if (Activator.CreateInstance(DefaultLoggerType) is not IMediaSearchLogger NewLogger) {
          throw new ApplicationException("Default logger is missing or invalid");
        }
        IMediaSearchLogger<T> NewDefaultLogger = _MakeLogger<T>(NewLogger);
        AddDefaultLogger(NewDefaultLogger);
        return NewDefaultLogger;
      }

      IMediaSearchLogger? DefaultLogger = _Items.SingleOrDefault(l => l.Name.Equals(DEFAULT_LOGGER_NAME, StringComparison.InvariantCultureIgnoreCase));
      if (DefaultLogger is not null) {
        return _MakeLogger<T>(DefaultLogger);
      } else {
        if (Activator.CreateInstance(DefaultLoggerType) is not IMediaSearchLogger NewLogger) {
          throw new ApplicationException("Default logger is missing or invalid");
        }
        IMediaSearchLogger<T> NewDefaultLogger = _MakeLogger<T>(NewLogger);
        AddDefaultLogger(NewDefaultLogger);
        return NewDefaultLogger;
      }
    }
  }

  private static IMediaSearchLogger<T> _MakeLogger<T>(IMediaSearchLogger? source) {
    return source switch {
      TMediaSearchLoggerFile FileLogger => new TMediaSearchLoggerFile<T>(FileLogger),
      TMediaSearchLoggerConsole ConsoleLogger => new TMediaSearchLoggerConsole<T>(ConsoleLogger),
      _ => throw new ApplicationException($"Invalid logger<T> type : {source?.GetType().GetGenericName() ?? "(unknonw)"}")
    };
  }

  public IMediaSearchLogger<T> GetLogger<T>() {
    lock (_Lock) {
      if (_Items.IsEmpty()) {
        return new TMediaSearchLoggerConsole<T>();
      }

      IMediaSearchLogger<T>? RetVal = _Items.OfType<IMediaSearchLogger<T>>().SingleOrDefault(l => l.Name.Equals(typeof(T).FullName, StringComparison.InvariantCultureIgnoreCase));
      if (RetVal is not null) {
        return RetVal;
      }

      RetVal = GetDefaultLogger<T>();
      if (RetVal is not null) {
        return RetVal;
      }

      throw new ApplicationException("Default logger is missing or invalid");
    }
  }

  public void Clear() {
    lock (_Lock) {
      _Items.Clear();
    }
  }

}
