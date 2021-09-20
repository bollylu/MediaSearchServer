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

namespace MovieSearchServer {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {

      services.AddSingleton<BLTools.Diagnostic.Logging.ILogger>(new TConsoleLogger());

      TMovieService MovieService = new TMovieService() {
        Storage = OperatingSystem.IsWindows() ? @"\\andromeda.sharenet.priv\films\" : @"/volume1/Films/"
      };
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
