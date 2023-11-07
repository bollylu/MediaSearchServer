using BLTools;
using BLTools.Diagnostic.Logging;

using MediaSearch.Models;

using static BLTools.ConsoleExtension.ConsoleExtension;
using static BLTools.Diagnostic.TraceInfo;

namespace BuildMoviesJson;

class Program {

  static ILogger Logger = new TConsoleLogger<Program>();

  static async Task Main(string[] args) {

    #region --- Constants --------------------------------------------
    int TIMEOUT_IN_MS = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
    #endregion --- Constants --------------------------------------------

    #region --- Parameters --------------------------------------------
    const string PARAM_DATASOURCE = "datasource";
    const string PARAM_DB_PATH = "dbpath";
    const string PARAM_DB_NAME = "dbname";
    const string PARAM_TABLE_NAME = "table";
    const string PARAM_LOG_FILENAME = "log";
    const string PARAM_LOG_SEVERITY = "severity";

    const string PARAM_DEFAULT_DATASOURCE = @"\\multimedia.sharenet.priv\films";
    const string PARAM_DEFAULT_DB_PATH = ".";
    const string PARAM_DEFAULT_DB_NAME = "demo";
    const string PARAM_DEFAULT_TABLE_NAME = "movies";
    const string PARAM_DEFAULT_LOG_FILENAME = "BuildMovies.log";
    const string PARAM_DEFAULT_LOG_SEVERITY = "info";

    ISplitArgs Args = new SplitArgs();
    Args.Parse(args);

    string ParamDataSource = Args.GetValue(PARAM_DATASOURCE, PARAM_DEFAULT_DATASOURCE);
    if (ParamDataSource == "" || !Directory.Exists(ParamDataSource)) {
      Usage($"Invalid or missing datasource : {ParamDataSource.WithQuotes()}");
    }

    string ParamDbPath = Args.GetValue(PARAM_DB_PATH, PARAM_DEFAULT_DB_PATH);
    string ParamDbName = Args.GetValue(PARAM_DB_NAME, PARAM_DEFAULT_DB_NAME);
    string ParamTableName = Args.GetValue(PARAM_TABLE_NAME, PARAM_DEFAULT_TABLE_NAME);

    string LogFileName = Args.GetValue(PARAM_LOG_FILENAME, PARAM_DEFAULT_LOG_FILENAME);
    string LogSeverity = Args.GetValue(PARAM_LOG_SEVERITY, PARAM_DEFAULT_LOG_SEVERITY);
    Logger = new TFileLogger(LogFileName) { SeverityLimit = Enum.Parse<ESeverity>(LogSeverity, true) };
    MediaSearch.Models.GlobalSettings.LoggerPool.AddLogger(new TFileLogger<TMediaMovieParserWindows>(LogFileName) { SeverityLimit = Enum.Parse<ESeverity>(LogSeverity, true) });

    Message($"{nameof(ParamDataSource)} = {ParamDataSource.WithQuotes()}");
    Message($"{nameof(ParamDbPath)} = {ParamDbPath.WithQuotes()}");
    Message($"{nameof(ParamDbName)} = {ParamDbName.WithQuotes()}");
    Message($"{nameof(ParamTableName)} = {ParamTableName.WithQuotes()}");

    #endregion --- Parameters --------------------------------------------

    //TMovie TestMovie = new TMovie();



    //#region --- Initialize storage --------------------------------------------
    //Message("Instanciate storage");
    //IStorageMovie Storage = new TStorageMemoryMovies() { PhysicalDataPath = ParamDataSource };

    //try {
    //  Message("Check for storage, if exists, remove it");
    //  if (await Storage.Exists()) {
    //    await Storage.Remove();
    //  }
    //  Message("Create storage");
    //  await Storage.Create();
    //} catch (Exception ex) {
    //  Usage($"Problem accessing storage : {ex.Message}");
    //}
    //Dump(Storage);
    //#endregion --- Initialize storage --------------------------------------------

    List<IMedia> Storage = new List<IMedia>();

    IMediaMovieParser MovieParser = new TMediaMovieParserWindows(ParamDataSource);
    MovieParser.Init();
    MovieParser.OnParseFolderProgress += MovieParser_OnParseFolderProgress;
    MovieParser.OnParseFolderStarting += MovieParser_OnParseFolderStarting;
    MovieParser.OnParseFolderCompleted += MovieParser_OnParseFolderCompleted;
    MovieParser.OnParseFileStarting += MovieParser_OnParseFileStarting;
    //MovieParser.OnParseFileCompleted += MovieParser_OnParseFileCompleted;

    using (TChrono Chrono = new()) {
      Chrono.Reset();
      Task DogetResults = Task.Run(() => {
        while (MovieParser.Results.Any() || !MovieParser.ParsingComplete) {
          MovieParser.Results.TryDequeue(out IMediaMovie? MovieItem);
          if (MovieItem is not null) {
            Storage.Add(MovieItem);
          }
        }
      });

      await Task.WhenAll(MovieParser.ParseFolderAsync(ParamDataSource), DogetResults);

      Message($"{Storage.Count} movies available");
      Message($"{MovieParser.LastParseCount} parses done");
      Message($"{MovieParser.LastSuccessCount} success");
      Message($"{MovieParser.LastErrorCount} errors");
      Chrono.Stop();
      Message($"Initialization done in {Chrono.ElapsedTime.DisplayTime()}");
    }

    await Pause();
    Environment.Exit(0);
  }

  private static void MovieParser_OnParseFileStarting(object? sender, string e) {
    MessageToConsole = $"Parsing {e.WithQuotes()} ...";
    ConsoleProgressUpdate();
  }

  private static void MovieParser_OnParseFileCompleted(object? sender, string e) {
    MessageToConsole = $"Parsing {e.WithQuotes()} completed";
    ConsoleProgressUpdate();
  }

  private static void MovieParser_OnParseFolderCompleted(object? sender, string e) {
    MessageToConsole = $"Parsing folder {e.WithQuotes()} completed";
    ConsoleProgressUpdate();
  }

  private static void MovieParser_OnParseFolderStarting(object? sender, string e) {
    MessageToConsole = $"Parsing folder {e.WithQuotes()} ...";
    ConsoleProgressUpdate();
  }

  private static void MovieParser_OnParseFolderProgress(object? sender, int e) {
    Counter = e;
    //ConsoleProgressUpdate();
  }

  static void Usage(string? message) {
    if (!string.IsNullOrWhiteSpace(message)) {
      Console.WriteLine(message);
    }

    Console.WriteLine("Usage: BuildMoviesJson /dbpath=<database path>");
    Console.WriteLine("                       /dbname=<database name>");
    Console.WriteLine("                       /datasource=<path to the data source>");
    Environment.Exit(1);
  }

  static string MessageToConsole = "";
  static int Counter = 0;

  static readonly object DisplayLock = new object();
  static void ConsoleProgressUpdate() {
    lock (DisplayLock) {
      int PosX = Console.CursorLeft;
      int PosY = Console.CursorTop;
      ConsoleColor BGC = Console.BackgroundColor;
      ConsoleColor FGC = Console.ForegroundColor;
      Console.SetCursorPosition(0, Console.CursorTop - 1);
      Console.BackgroundColor = ConsoleColor.Cyan;
      Console.ForegroundColor = ConsoleColor.DarkBlue;
      Console.Write($"{Counter:0000} => {MessageToConsole}".AlignedLeft(Console.BufferWidth));
      Console.BackgroundColor = BGC;
      Console.ForegroundColor = FGC;
      Console.SetCursorPosition(PosX, PosY);
    }
  }
}