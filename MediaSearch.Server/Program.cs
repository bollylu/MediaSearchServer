using BLTools.Diagnostic.Logging;

using MediaSearch.Server.Controllers;

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
  public const string ARG_AUDITFILE = "audit";

  public const string DEFAULT_LOGFILE_LINUX = "/var/log/MediaSearch/MediaSearch.Server.log";
  public const string DEFAULT_LOGFILE_WINDOWS = @"c:\logs\MediaSearch\MediaSearch.Server.log";

  public const string DEFAULT_AUDITFILE_LINUX = "/var/log/MediaSearch/MediaSearch.Server.audit";
  public const string DEFAULT_AUDITFILE_WINDOWS = @"c:\logs\MediaSearch\MediaSearch.Server.audit";
  #endregion --- Parameters --------------------------------------------

  #region --- Global variables --------------------------------------------
  public static IConfiguration? Configuration { get; private set; }
  #endregion --- Global variables --------------------------------------------

  const string DEFAULT_SERVER_NAME = "http://localhost:4567";

  #region -----------------------------------------------
  public static void Main(string[] args) {

    if (OperatingSystem.IsWindows()) {
      Console.SetWindowSize(132, 50);
    }

    GlobalSettings.AppArgs.Parse(args);

    if (GlobalSettings.AppArgs.IsDefined(ARG_HELP) || GlobalSettings.AppArgs.IsDefined(ARG_HELP2)) {
      Usage();
    }

    #region --- Log configuration --------------------------------------------
    string LogFile = GlobalSettings.AppArgs.GetValue("log", OperatingSystem.IsWindows() ? DEFAULT_LOGFILE_WINDOWS : DEFAULT_LOGFILE_LINUX);

    GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerFile(LogFile) { SeverityLimit = ESeverity.Debug });
    GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<Program>(LogFile) { SeverityLimit = ESeverity.Debug });
    GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TLoginController>(LogFile) { SeverityLimit = ESeverity.Debug });
    GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TMovieController>(LogFile) { SeverityLimit = ESeverity.Debug });
    GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TSystemController>(LogFile) { SeverityLimit = ESeverity.Debug });

    IMediaSearchLogger<Program> Logger = GlobalSettings.LoggerPool.GetLogger<Program>();

    MediaSearch.Models.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerFile(LogFile) { SeverityLimit = ESeverity.Debug });
    MediaSearch.Models.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TAbout>(LogFile) { SeverityLimit = ESeverity.Debug });

    MediaSearch.Server.Services.GlobalSettings.LoggerPool.AddDefaultLogger(new TMediaSearchLoggerFile(LogFile) { SeverityLimit = ESeverity.Debug });
    MediaSearch.Server.Services.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TLoginService>(LogFile) { SeverityLimit = ESeverity.Debug });
    MediaSearch.Server.Services.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TMovieService>(LogFile) { SeverityLimit = ESeverity.Debug });
    MediaSearch.Server.Services.GlobalSettings.LoggerPool.AddLogger(new TMediaSearchLoggerFile<TMSTableJsonMovie>(LogFile) { SeverityLimit = ESeverity.Debug });
    #endregion --- Log configuration --------------------------------------------

    string AuditFile = GlobalSettings.AppArgs.GetValue("audit", OperatingSystem.IsWindows() ? DEFAULT_AUDITFILE_WINDOWS : DEFAULT_AUDITFILE_LINUX);
    IAuditService AuditService = new TAuditServiceFile(Path.GetDirectoryName(Path.GetFullPath(AuditFile)) ?? "./", Path.GetFileName(AuditFile));
    MediaSearch.Server.GlobalSettings.AuditService = AuditService;
    MediaSearch.Server.Services.GlobalSettings.AuditService = AuditService;

    GlobalSettings.AuditService.Audit("", "Server startup");

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
    Logger.Log(GlobalSettings.ListAbout());

    if (GlobalSettings.AppArgs.IsDefined(ARG_VERBOSE) || Configuration.GetSection(ARG_VERBOSE).Exists()) {
      Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("From Main", GlobalSettings.DEBUG_BOX_WIDTH));
      Logger.LogBox("Config from Main", Configuration.DumpConfig());
    }

    CreateHostBuilder(args).Build().Run();

    GlobalSettings.AuditService.Audit("", "Server stopped gracefully.");
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
    Console.WriteLine("  [audit=<auditfile path and name>]");
    Console.WriteLine("  [datasource=<root data source>]");

    Environment.Exit(1);
  }
}
