using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using BLTools.Diagnostic.Logging;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

using MovieSearchServerServices.MovieService;

using ILogger = BLTools.Diagnostic.Logging.ILogger;

namespace MovieSearchServer {
  public class Startup {

    public static ILogger Logger { get; private set; }

    public const string DEFAULT_DATASOURCE = @"\\andromeda.sharenet.priv\multimedia\films\";
    public const string DEFAULT_LOGFILE_LINUX = "/var/log/MovieSearch/MovieSearchServer.log";
    public const string DEFAULT_LOGFILE_WINDOWS = @"c:\logs\MovieSearch\MovieSearchServer.log";
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

      string DataSource = Program.AppArgs.GetValue("datasource", DEFAULT_DATASOURCE);
      string LogFile = OperatingSystem.IsWindows() ? Program.AppArgs.GetValue("log", DEFAULT_LOGFILE_WINDOWS) : Program.AppArgs.GetValue("log", DEFAULT_LOGFILE_LINUX);

      Logger = new TFileLogger(LogFile);
      Logger.Log("MovieSearchServer startup...");

      services.AddSingleton<ILogger>(Logger);

      TMovieService MovieService = new TMovieService() {
        Storage = DataSource
      };

      MovieService.SetLogger(Logger);
      Task.Run(async () => await MovieService.Initialize());

      services.AddSingleton<IMovieService>(MovieService);

      services.AddCors(options => {
        options.AddPolicy("AllowAll", policy => policy.AllowAnyOrigin()//.WithOrigins("http://10.100.200.7")
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
      if (env.IsDevelopment()) {
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
}
