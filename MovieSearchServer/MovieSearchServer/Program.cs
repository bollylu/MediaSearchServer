using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MovieSearchServer {
  public class Program {

    const string SERVER_CERTIFICATE = "Certificates\\lucwks7.sharenet.priv.pem";
    const string SERVER_CERTIFICATE_KEY = "Certificates\\lucwks7.sharenet.priv.key";

    public static void Main(string[] args) {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>

        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => {
              if (OperatingSystem.IsLinux()) {
                webBuilder.UseStartup<Startup>()
                          .UseUrls("http://multimedia.sharenet.priv:4567");
              } else {
                //if (!File.Exists(SERVER_CERTIFICATE)) {
                //  Console.WriteLine("Missing certificate");
                //}
                webBuilder.UseStartup<Startup>()
                          //.UseKestrel(options => {
                          //  options.ConfigureHttpsDefaults(options => {
                          //    X509Certificate2 Cert = X509Certificate2.CreateFromPemFile(SERVER_CERTIFICATE, SERVER_CERTIFICATE_KEY);
                          //    options.ServerCertificate = Cert;
                          //  });
                          //})
                          //.UseUrls("https://lucwks7.sharenet.priv:4567")
                          ;
              }
            });
  }
}
