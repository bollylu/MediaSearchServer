using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System.Reflection;

namespace MediaSearch.Server;

public class Program {

  #region --- Parameters --------------------------------------------
  public const string ARG_HELP = "help";
  public const string ARG_HELP2 = "?";
  public const string ARG_VERBOSE = "verbose";
  public const string ARG_LOGFILE = "log";
  public const string ARG_CHANGELOG = "changelog";
  public const string ARG_DATA_SOURCE = "datasource";

  public const string DEFAULT_LOGFILE_LINUX = "/var/log/MediaSearch/MediaSearch.Server.log";
  public const string DEFAULT_LOGFILE_WINDOWS = @"c:\logs\MediaSearch\MediaSearch.Server.log";
  #endregion --- Parameters --------------------------------------------

  #region --- Global variables --------------------------------------------
  
  public static IConfiguration? Configuration { get; private set; }

  public static TGlobalSettings GlobalSettings { get; } = new();
  #endregion --- Global variables --------------------------------------------

  const string DEFAULT_SERVER_NAME = "http://localhost:4567";

  #region -----------------------------------------------
  public static async Task Main(string[] args) {

    if (OperatingSystem.IsWindows()) {
      Console.SetWindowSize(132, 50);
    }

    GlobalSettings.AppArgs.Parse(args);

    if (GlobalSettings.AppArgs.IsDefined(ARG_HELP) || GlobalSettings.AppArgs.IsDefined(ARG_HELP2)) {
      Usage();
    }
    
    await GlobalSettings.Initialize();

    string LogFile = Program.GlobalSettings.AppArgs.GetValue("log", OperatingSystem.IsWindows() ? DEFAULT_LOGFILE_WINDOWS : DEFAULT_LOGFILE_LINUX);
    Program.GlobalSettings.GlobalLogger = new TFileLogger(LogFile) { SeverityLimit = ESeverity.Debug };

    MediaSearch.Models.GlobalSettings.GlobalLogger = ALogger.Create(Program.GlobalSettings.GlobalLogger);
    MediaSearch.Models.GlobalSettings.GlobalLogger.SeverityLimit = ESeverity.DebugEx;

    #region --- Configuration --------------------------------------------
    IConfigurationBuilder ConfigurationBuilder = new ConfigurationBuilder();
    ConfigurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
    ConfigurationBuilder.AddJsonFile("appsettings.json");
    ConfigurationBuilder.AddJsonFile("appsettings.Development.json");
    if (OperatingSystem.IsWindows()) {
      ConfigurationBuilder.AddJsonFile("appsettings.windows.json");
    } else {
      ConfigurationBuilder.AddJsonFile("appsettings.linux.json");
    }
    ConfigurationBuilder.AddCommandLine(args);
    Configuration = ConfigurationBuilder.Build();
    #endregion --- Configuration --------------------------------------------

    Console.WriteLine(GlobalSettings.ListAbout());

    if (GlobalSettings.AppArgs.IsDefined(ARG_VERBOSE) || Configuration.GetSection(ARG_VERBOSE).Exists()) {
      Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("From Main", TGlobalSettings.DEBUG_BOX_WIDTH));
    }

    CreateHostBuilder(args).Build().Run();
  }
  #endregion -----------------------------------------------

  public static IHostBuilder CreateHostBuilder(string[] args) {

    string Server = Configuration.GetValue("server", DEFAULT_SERVER_NAME);
    if (!(Server.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) ||
          Server.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))) {
      Usage($"Invalid [server] parameter : {Server} is invalid, default to {DEFAULT_SERVER_NAME}");
      Server = DEFAULT_SERVER_NAME;
    }


    return Host.CreateDefaultBuilder(args)
               .ConfigureAppConfiguration(config => { config.AddConfiguration(Configuration); })
               .ConfigureWebHostDefaults(webBuilder => {
                 webBuilder.UseStartup<Startup>()
                         //.UseKestrel(options => {
                         //  options.ConfigureHttpsDefaults(options => {
                         //    X509Certificate2 Cert = X509Certificate2.CreateFromPemFile(SERVER_CERTIFICATE, SERVER_CERTIFICATE_KEY);
                         //    options.ServerCertificate = Cert;
                         //  });
                         //})
                         .UseKestrel()
                         .UseUrls(Server)
                         ;
               });
  }

  public static void Usage(string message = "") {
    if (message != "") {
      Console.WriteLine(message);
    }

    Console.WriteLine($"MediaSearch.Server v{GlobalSettings.EntryAbout?.CurrentVersion ?? new Version()}");
    Console.WriteLine("Usage : ./MediaSearch.Server [params]");
    Console.WriteLine("  [server=<server ip or dns>]");
    Console.WriteLine("  [log=<logfile path and name>]");
    Console.WriteLine("  [datasource=<root data source>]");

    Environment.Exit(1);
  }
}
