using BLTools.Diagnostic.Logging;

namespace MediaSearch.Models.Logging;

public class TLoggerPool : BLTools.Diagnostic.Logging.TLoggerPool {

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public TLoggerPool() {
    DefaultLogger = new TMediaSearchConsoleLogger();
  }

  public TLoggerPool(IEnumerable<IMediaSearchLogger> Items) : this() {
    _Items.AddRange(Items);
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------


  public override IMediaSearchLogger<T> GetDefaultLogger<T>() {

    if (_Items.IsEmpty()) {
      IMediaSearchLogger? NewLogger = Activator.CreateInstance(DefaultLoggerType) as IMediaSearchLogger;
      if (NewLogger is not null) {
        IMediaSearchLogger<T> NewDefaultLogger = _MakeLogger<T>(NewLogger);
        AddDefaultLogger(NewDefaultLogger);
        return NewDefaultLogger;
      } else {
        throw new ApplicationException("Default logger is missing or invalid");
      }
    }

    IMediaSearchLogger? DefaultLogger = _Items.OfType<IMediaSearchLogger>().SingleOrDefault(l => l.Name.Equals(DEFAULT_LOGGER_NAME, StringComparison.InvariantCultureIgnoreCase));
    if (DefaultLogger is not null) {
      return DefaultLogger switch {
        TMediaSearchFileLogger Logger => new TMediaSearchLoggerFile<T>(Logger),
        TMediaSearchConsoleLogger Logger => new TMediaSearchLoggerConsole<T>(Logger),
        _ => throw new ApplicationException("Invalid logger<T> type")
      };
    } else {
      IMediaSearchLogger? NewLogger = Activator.CreateInstance(DefaultLoggerType) as IMediaSearchLogger;
      if (NewLogger is not null) {
        IMediaSearchLogger<T> NewDefaultLogger = _MakeLogger<T>(NewLogger);
        AddDefaultLogger(NewDefaultLogger);
        return NewDefaultLogger;
      } else {
        throw new ApplicationException("Default logger is missing or invalid");
      }
    }

  }

  public override IMediaSearchLogger<T> GetLogger<T>() {
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

  protected override ILogger<T> _MakeLogger<T>(ILogger source) where T : class {
    return source switch {
      TMediaSearchFileLogger FileLogger => new TMediaSearchLoggerFile<T>(FileLogger),
      TMediaSearchConsoleLogger ConsoleLogger => new TMediaSearchLoggerConsole<T>(ConsoleLogger),
      _ => throw new ApplicationException("Invalid logger<T> type : {source?.GetType().GetNameEx() ?? \"(unknown)\"}")
    };
  }

}
