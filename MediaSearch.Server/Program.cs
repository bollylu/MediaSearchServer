using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

using System.Reflection;

namespace MediaSearch.Server;

public class Program {

  #region --- Parameter names --------------------------------------------
  public const string ARG_HELP = "help";
  public const string ARG_HELP2 = "?";
  public const string ARG_VERBOSE = "verbose";
  public const string ARG_LOGFILE = "log";
  public const string ARG_CHANGELOG = "changelog";
  public const string ARG_DATA_SOURCE = "datasource";
  #endregion --- Parameter names --------------------------------------------

  #region --- Global variables --------------------------------------------
  public static ISplitArgs AppArgs { get; } = new SplitArgs();
  public static IConfiguration? Configuration { get; private set; }

  public static List<IAbout> About { get; } = new();
  public static IAbout EntryAbout => TAbout.Entry;
  #endregion --- Global variables --------------------------------------------

  const string DEFAULT_SERVER_NAME = "http://localhost:4567";

  #region -----------------------------------------------
  public static async Task Main(string[] args) {

    if (OperatingSystem.IsWindows()) {
      Console.SetWindowSize(132, 50);
    }

    AppArgs.Parse(args);

    if (AppArgs.IsDefined(ARG_HELP) || AppArgs.IsDefined(ARG_HELP2)) {
      Usage();
    }

    foreach (Assembly AssemblyItem in AppDomain.CurrentDomain.GetAssemblies().Where(a => (a.GetName()?.Name ?? "").StartsWith("MediaSearch"))) {
      IAbout AssemblyAbout = new TAbout(AssemblyItem);
      await AssemblyAbout.Initialize();
      About.Add(AssemblyAbout);
    }

    await EntryAbout.Initialize().ConfigureAwait(false);

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

    foreach (IAbout AboutItem in About) {
      Console.WriteLine(AboutItem.CurrentVersion.ToString().BoxFixedWidth($"{AboutItem.Name} version #", GlobalSettings.DEBUG_BOX_WIDTH));
      if (AppArgs.IsDefined(ARG_CHANGELOG)) {
        Console.WriteLine(AboutItem.ChangeLog.BoxFixedWidth($"Change log {AboutItem.Name}", GlobalSettings.DEBUG_BOX_WIDTH));
      }
    }

    if (AppArgs.IsDefined(ARG_VERBOSE) || Configuration.GetSection(ARG_VERBOSE).Exists()) {
      Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("From Main", GlobalSettings.DEBUG_BOX_WIDTH));
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

    Console.WriteLine($"MediaSearch.Server v{EntryAbout?.CurrentVersion ?? new Version()}");
    Console.WriteLine("Usage : ./MediaSearch.Server [params]");
    Console.WriteLine("  [server=<server ip or dns>]");
    Console.WriteLine("  [log=<logfile path and name>]");
    Console.WriteLine("  [datasource=<root data source>]");

    Environment.Exit(1);
  }
}
