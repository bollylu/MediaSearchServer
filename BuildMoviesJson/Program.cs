using BLTools;
using BLTools.Diagnostic;

using MediaSearch.Models;
using MediaSearch.Server.Services;
using MediaSearch.Storage;

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

    TraceInfo.Message($"{nameof(ParamDataSource)} = {ParamDataSource.WithQuotes()}");
    TraceInfo.Message($"{nameof(ParamDbPath)} = {ParamDbPath.WithQuotes()}");
    TraceInfo.Message($"{nameof(ParamDbName)} = {ParamDbName.WithQuotes()}");
    TraceInfo.Message($"{nameof(ParamTableName)} = {ParamTableName.WithQuotes()}");

    #endregion --- Parameters --------------------------------------------

    IStorage Database = new TStorageMemory();

    try {
      if (Database.Exists()) {
        Database.Remove();
      }
      Database.Create();
    } catch (Exception ex) {
      Usage($"Problem accessing database : {ex.Message}");
    }

    Database.Open();
    IMSTable<IMovie> MovieTable = new TMSTable<IMovie>(ParamTableName);
    IMediaSource<IMovie> MovieSource = new TMediaSource<IMovie>(ParamDataSource);
    MovieTable.Header.SetMediaSource(MovieSource);
    Database.TableCreate(MovieTable);

    TraceInfo.DumpWithMessage("Database", Database);

    IMovieService MovieService = new TMovieService(MovieTable);

    TraceInfo.Dump(MovieService);

    using (TChrono Chrono = new()) {
      await Chrono.ExecuteTaskAsync(MovieService.Initialize()).ConfigureAwait(false);
      TraceInfo.Message($"Initialization done in {Chrono.ElapsedTime.DisplayTime()}");
    }

    Database.Close();
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