using System.Text;

using BLTools;
using BLTools.Text;

using MediaSearch.Models;
using MediaSearch.Server.Services;
using static BuildMoviesJson.Display;

namespace DisplayStdin {
  class Program {
    static void Main(string[] args) {

      int TIMEOUT_IN_MS = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;

      const string PARAM_DATASOURCE = "source";
      const string PARAM_DB_PATH = "dbpath";
      const string PARAM_DB_NAME = "dbname";

      ISplitArgs Args = new SplitArgs();
      Args.Parse(args);

      string DataSource = Args.GetValue(PARAM_DATASOURCE, "");
      if (DataSource == "" || !Directory.Exists(DataSource)) {
        Usage($"Invalid or missing datasource : {DataSource.WithQuotes()}");
      }

      string DbPath = Args.GetValue(PARAM_DB_PATH, ".");
      string DbName = Args.GetValue(PARAM_DB_NAME, "movies");
      IMediaSearchDataTable Database = new TMSTableJsonMovie(DbPath,DbName);
      
      try {
        if (Database.Exists()) {
          Database.Remove();
        }
        Database.Create();
        Database.OpenOrCreate();
      } catch (Exception ex) {
        Usage($"Problem accessing database : {ex.Message}");
      }
      TraceMessage("db file", Database);

      IMovieService MovieService = new TMovieService(Database) { RootStoragePath = DataSource };

      using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
        MovieService.Parse();
      }

      //using (CancellationTokenSource Timeout = new CancellationTokenSource(TIMEOUT_IN_MS)) {
        //await Database.SaveAsync(Timeout.Token).ConfigureAwait(false);
        Database.Save();
      //}

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
}