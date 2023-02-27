using MediaSearch.Storage;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace MediaSearch.Server;

public class Startup {

  public const string DEFAULT_DATASOURCE = @"\\andromeda.sharenet.priv\multimedia\films\";
  public readonly ILogger Logger = GlobalSettings.LoggerPool.GetLogger<Startup>();

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public Startup(IConfiguration configuration) {
    Configuration = configuration;
    if (GlobalSettings.AppArgs.IsDefined(Program.ARG_VERBOSE)) {
      Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("Configuration in Startup constructor", GlobalSettings.DEBUG_BOX_WIDTH));
    }
  }
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services) {

    #region --- Log startup info --------------------------------------------
    StringBuilder StartupInfo = new StringBuilder();
    StartupInfo.AppendLine($"Version {GlobalSettings.EntryAbout.CurrentVersion}");
    StartupInfo.AppendLine($"Running for {Environment.UserName}");
    StartupInfo.AppendLine($"Running on {Environment.MachineName}");
    StartupInfo.AppendLine($"Runtime version {Environment.Version}");
    StartupInfo.AppendLine($"OS version {Environment.OSVersion}");
    Logger.LogBox("Startup info", StartupInfo);
    #endregion --- Log startup info --------------------------------------------

    Logger.Log("MediaSearch.Server startup...");
    string DataSource;
    if (Configuration is not null) {
      DataSource = Configuration.GetValue("datasource", DEFAULT_DATASOURCE) ?? DEFAULT_DATASOURCE;
    } else {
      DataSource = DEFAULT_DATASOURCE;
    }
    Logger.Log($"Data source is {DataSource}");

    services.AddControllers(options => {
      options.OutputFormatters.Insert(0, new TJsonOutputFormatter());
      options.InputFormatters.Insert(0, new TJsonInputFormatter());
    });

    IMediaSource? MediaSource = TMediaSource.Create(DataSource, typeof(IMovie));
    if (MediaSource is null) {
      Logger.LogErrorBox("Unable to create data source", DataSource);
    }

    IMovieService MovieService = new TMovieService(new TStorageMemoryMovies(), MediaSource);
    Task.Run(async () => await MovieService.Initialize());
    services.AddSingleton<IMovieService>(MovieService);

    ILoginService LoginService = new TLoginService();
    services.AddSingleton(LoginService);

    services.AddCors(options => {
      options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin()
                                                                  .AllowAnyMethod()
                                                                  .AllowAnyHeader()
                       );
    });
    services.AddControllers();
    services.AddSwaggerGen(c => {
      c.SwaggerDoc("v1", new OpenApiInfo {
        Title = "MediaSearch.Server",
        Version = GlobalSettings.EntryAbout.CurrentVersion.ToString()
      }
      );
      c.SchemaFilter<MySwaggerSchemaFilter>();
    });

    Logger.Log($"MediaSearch.Server {GlobalSettings.EntryAbout.CurrentVersion} startup complete. Running.");
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
    if (env.IsDevelopment()) {
      app.UseDeveloperExceptionPage();
      app.UseForwardedHeaders();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"MediaSearch.Server v{GlobalSettings.EntryAbout.CurrentVersion}"));
    }

    app.UseCors("AllowAll");

    app.UseRouting();

    //app.UseAuthorization();

    app.UseEndpoints(endpoints => {
      endpoints.MapControllers();
      endpoints.MapFallbackToFile("index.html");
    });
  }


  public class MySwaggerSchemaFilter : ISchemaFilter {
    public void Apply(OpenApiSchema schema, SchemaFilterContext context) {
      if (schema is null || schema.Properties is null) {
        return;
      }

      RemoveFromSchema(context.SchemaRepository.Schemas, "ILogger");
      RemoveFromSchema(context.SchemaRepository.Schemas, "TraceOptions");
      RemoveFromSchema(context.SchemaRepository.Schemas, "TraceListener");
      RemoveFromSchema(context.SchemaRepository.Schemas, "TraceFilter");
      RemoveFromSchema(context.SchemaRepository.Schemas, "ESeverity");
      RemoveFromSchema(context.SchemaRepository.Schemas, "DayOfWeek");
      RemoveFromSchema(context.SchemaRepository.Schemas, "DateOnly");
      RemoveFromSchema(context.SchemaRepository.Schemas, "Version");

    }

    private static void RemoveFromSchema(Dictionary<string, OpenApiSchema> schemas, string key) {
      if (schemas.ContainsKey(key)) {
        schemas.Remove(key);
      }
    }
  }



}
