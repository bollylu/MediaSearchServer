using BLTools.Text;

using MediaSearch.Server.Support;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace MediaSearch.Server;

public class Startup {

  public static ILogger Logger { get; private set; }

  public const string DEFAULT_DATASOURCE = @"\\andromeda.sharenet.priv\multimedia\films\";
  public const string DEFAULT_LOGFILE_LINUX = "/var/log/MovieSearch/MovieSearchServer.log";
  public const string DEFAULT_LOGFILE_WINDOWS = @"c:\logs\MovieSearch\MovieSearchServer.log";

  public Startup(IConfiguration configuration) {
    Configuration = configuration;
    Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("From Startup", 160));
  }

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services) {

    string DataSource = Configuration.GetValue("datasource", DEFAULT_DATASOURCE);
    string LogFile = Program.AppArgs.GetValue("log", OperatingSystem.IsWindows() ? DEFAULT_LOGFILE_WINDOWS : DEFAULT_LOGFILE_LINUX);

    Logger = new TFileLogger(LogFile) { SeverityLimit = ESeverity.Debug };
    StringBuilder StartupInfo = new StringBuilder();
    StartupInfo.AppendLine($"Version {Program.About.CurrentVersion}");
    StartupInfo.AppendLine($"Running for {Environment.UserName}");
    StartupInfo.AppendLine($"Running on {Environment.MachineName}");
    StartupInfo.AppendLine($"Runtime version {Environment.Version}");
    StartupInfo.AppendLine($"OS version {Environment.OSVersion}");
    Logger.Log(TextBox.BuildFixedWidth(StartupInfo.ToString(), "Startup info", 80, TextBox.EStringAlignment.Left));
    //Console.WriteLine("Press any key to continue...");
    //Console.ReadKey();

    Logger.Log("MediaSearchServer startup...");
    Logger.Log($"Data source is {DataSource}");

    services.AddSingleton<ILogger>(Logger);

    services.AddControllers(options => {
      options.OutputFormatters.Insert(0, new TJsonOutputFormatter());
      //options.OutputFormatters.Insert(0, new TMovieOutputFormatter());
      //options.OutputFormatters.Insert(0, new TMoviesPageOutputFormatter());
    });

    TMovieService MovieService = new TMovieService(DataSource);

    MovieService.SetLogger(Logger);
    Task.Run(async () => await MovieService.Initialize());

    services.AddSingleton<IMovieService>(MovieService);

    services.AddCors(options => {
      options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin()
                                                    .AllowAnyMethod()
                                                    .AllowAnyHeader()
                       );
    });
    services.AddControllers();
    services.AddSwaggerGen(c => {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "MovieSearchServer", Version = "v1" });
    });

    Logger.Log("MovieSearchServer startup complete. Running.");
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
    if ( env.IsDevelopment() ) {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieSearchServer v1"));
    }

    app.UseCors("AllowAll");

    //app.UseHttpsRedirection();
    
    app.UseRouting();

    app.UseAuthorization();

    app.UseEndpoints(endpoints => {
      endpoints.MapControllers();
      endpoints.MapFallbackToFile("index.html");
    });
  }
}
