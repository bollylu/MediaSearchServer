using BLTools;
using BLTools.Diagnostic.Logging;

using MediaSearch.Models;
using MediaSearch.Storage;

using static BLTools.ConsoleExtension.ConsoleExtension;
using static BLTools.Diagnostic.TraceInfo;

namespace BuildMoviesJson;

class Program {
  static async Task Main(string[] args) {

    #region --- Constants --------------------------------------------
    int TIMEOUT_IN_MS = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
    #endregion --- Constants --------------------------------------------

    #region --- Parameters --------------------------------------------
    const string PARAM_DATASOURCE = "datasource";
    const string PARAM_DB_PATH = "dbpath";
    const string PARAM_DB_NAME = "dbname";
    const string PARAM_TABLE_NAME = "table";

    const string PARAM_DEFAULT_DATASOURCE = @"\\multimedia.sharenet.priv\multimedia\films";
    const string PARAM_DEFAULT_DB_PATH = ".";
    const string PARAM_DEFAULT_DB_NAME = "demo";
    const string PARAM_DEFAULT_TABLE_NAME = "movies";

    ISplitArgs Args = new SplitArgs();
    Args.Parse(args);

    string? ParamDataSource = Args.GetValue(PARAM_DATASOURCE, PARAM_DEFAULT_DATASOURCE);
    if (ParamDataSource == "" || !Directory.Exists(ParamDataSource)) {
      Usage($"Invalid or missing datasource : {ParamDataSource.WithQuotes()}");
    }

    string ParamDbPath = Args.GetValue(PARAM_DB_PATH, PARAM_DEFAULT_DB_PATH);
    string ParamDbName = Args.GetValue(PARAM_DB_NAME, PARAM_DEFAULT_DB_NAME);
    string ParamTableName = Args.GetValue(PARAM_TABLE_NAME, PARAM_DEFAULT_TABLE_NAME);

    Message($"{nameof(ParamDataSource)} = {ParamDataSource.WithQuotes()}");
    Message($"{nameof(ParamDbPath)} = {ParamDbPath.WithQuotes()}");
    Message($"{nameof(ParamDbName)} = {ParamDbName.WithQuotes()}");
    Message($"{nameof(ParamTableName)} = {ParamTableName.WithQuotes()}");

    #endregion --- Parameters --------------------------------------------

    #region --- Initialize storage --------------------------------------------
    Message("Instanciate storage");
    IStorageMovie Storage = new TStorageMemoryMovies() { PhysicalDataPath = ParamDataSource };

    try {
      Message("Check for storage, if exists, remove it");
      if (Storage.Exists()) {
        Storage.Remove();
      }
      Message("Create storage");
      Storage.Create();
    } catch (Exception ex) {
      Usage($"Problem accessing storage : {ex.Message}");
    }
    Dump(Storage);
    #endregion --- Initialize storage --------------------------------------------

    ILogger Logger = new TConsoleLogger<Program>();
    IEnumerable<IFileInfo> AvailableMovies = Support.FetchFiles(ParamDataSource, CancellationToken.None);
    TCounter Counter = new();
    using (TChrono Chrono = new()) {
      Chrono.Reset();
      //await Parallel.ForEachAsync(AvailableMovies, new ParallelOptions() { MaxDegreeOfParallelism = 10 }, async (f, t) => {
      AvailableMovies.AsParallel().ForAll(async f => {
        IMovie? NewMovie = await TMovie.Parse(f, Storage.PhysicalDataPath, Logger).ConfigureAwait(false);
        if (NewMovie is not null) {
          await NewMovie.LoadPicture().ConfigureAwait(false);
          await Storage.AddMovieAsync(NewMovie).ConfigureAwait(false);
        }
        Counter.Increment();
      });

      Chrono.Stop();
      Message($"Initialization done in {Chrono.ElapsedTime.DisplayTime()}");
    }

    Dump(Storage);

    //using (TChrono Chrono = new()) {
    //  await Chrono.ExecuteTaskAsync(MovieService.Initialize()).ConfigureAwait(false);
    //  TraceInfo.Message($"Initialization done in {Chrono.ElapsedTime.DisplayTime()}");
    //}

    //Database.Close();
    await Pause();
    Environment.Exit(0);
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


}