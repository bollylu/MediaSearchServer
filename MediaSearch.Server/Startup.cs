using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System.Reflection;

namespace MediaSearch.Server;

public class Startup {

  public const string DEFAULT_DATASOURCE = @"\\andromeda.sharenet.priv\multimedia\films\";

  private readonly ILogger Logger;

  #region --- Constructor(s) ---------------------------------------------------------------------------------
  public Startup(IConfiguration configuration) {
    Logger = ALogger.Create(Program.GlobalSettings.GlobalLogger);
    Configuration = configuration;
    if (Program.GlobalSettings.AppArgs.IsDefined(Program.ARG_VERBOSE)) {
      Console.WriteLine(Configuration.DumpConfig().BoxFixedWidth("Configuration in Startup constructor", TGlobalSettings.DEBUG_BOX_WIDTH));
    }
  } 
  #endregion --- Constructor(s) ------------------------------------------------------------------------------

  public IConfiguration Configuration { get; }

  // This method gets called by the runtime. Use this method to add services to the container.
  public void ConfigureServices(IServiceCollection services) {

    string DataSource = Configuration.GetValue("datasource", DEFAULT_DATASOURCE);

    StringBuilder StartupInfo = new StringBuilder();
    StartupInfo.AppendLine($"Version {Program.GlobalSettings.EntryAbout?.CurrentVersion ?? new Version()}");
    StartupInfo.AppendLine($"Running for {Environment.UserName}");
    StartupInfo.AppendLine($"Running on {Environment.MachineName}");
    StartupInfo.AppendLine($"Runtime version {Environment.Version}");
    StartupInfo.AppendLine($"OS version {Environment.OSVersion}");
    Logger.Log(TextBox.BuildFixedWidth(StartupInfo.ToString(), "Startup info", 80, TextBox.EStringAlignment.Left));

    Logger.Log("MediaSearch.Server startup...");
    Logger.Log($"Data source is {DataSource}");

    services.AddSingleton<ILogger>(Logger);

    services.AddControllers(options => {
      options.OutputFormatters.Insert(0, new TJsonOutputFormatter());
      options.InputFormatters.Insert(0, new TJsonInputFormatter());
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
      c.SwaggerDoc("v1", new OpenApiInfo { 
                                        Title = "MediaSearch.Server", 
                                        Version = Program.GlobalSettings.EntryAbout?.CurrentVersion.ToString() 
                                   }
                  );
      //c.SchemaFilter<MySwaggerSchemaFilter>();
    });

    Logger.Log($"MediaSearch.Server {Program.GlobalSettings.EntryAbout?.CurrentVersion} startup complete. Running.");
  }

  // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
  public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
    if ( env.IsDevelopment() ) {
      app.UseDeveloperExceptionPage();
      app.UseSwagger();
      app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"MediaSearch.Server v{Program.GlobalSettings.EntryAbout.CurrentVersion}"));
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
      
      //RemoveFromSchema(context.SchemaRepository.Schemas, "ILogger");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "TraceOptions");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "TraceListener");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "TraceFilter");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "ESeverity");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "DayOfWeek");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "DateOnly");
      //RemoveFromSchema(context.SchemaRepository.Schemas, "Version");

    }

    private void RemoveFromSchema(Dictionary<string, OpenApiSchema> schemas, string key) {
      if (schemas.ContainsKey(key)) {
        schemas.Remove(key);
      }
    }
  }



}
