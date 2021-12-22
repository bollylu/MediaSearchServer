using BLTools.Text;

using MediaSearch.Server.Support;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace MediaSearch.Server;

public class Program {

  public const string ARG_VERBOSE = "verbose";
  public const string ARG_DATA_SOURCE = "datasource";

  public static readonly Version AppVersion = new Version(0, 0, 7);

  public static ISplitArgs AppArgs { get; private set; }
  public static IConfiguration Configuration { get; private set; }

  const string DEFAULT_SERVER_NAME = "http://localhost:4567";

  public static void Main(string[] args) {
    AppArgs = new SplitArgs();
    AppArgs.Parse(args);

    if ( AppArgs.IsDefined("?") || AppArgs.IsDefined("help") ) {
      Usage();
    }

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
    
    if ( AppArgs.IsDefined(ARG_VERBOSE) || Configuration.GetSection(ARG_VERBOSE).Exists() ) {
      Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("From Main", 160));
    }
    #endregion --- Configuration --------------------------------------------

    CreateHostBuilder(args).Build().Run();
  }

  public static IHostBuilder CreateHostBuilder(string[] args) {

    string Server = Configuration.GetValue("server", DEFAULT_SERVER_NAME);
    if ( !( Server.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase) ||
          Server.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase) ) ) {
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
    if ( message != "" ) {
      Console.WriteLine(message);
    }

    Console.WriteLine($"MovieSearchServer v{AppVersion}");
    Console.WriteLine("Usage : MovieSearchServer [params]");
    Console.WriteLine("  [server=<server ip or dns>]");
    Console.WriteLine("  [log=<logfile path and name>]");
    Console.WriteLine("  [datasource=<root data source>]");

    Environment.Exit(1);
  }
}
