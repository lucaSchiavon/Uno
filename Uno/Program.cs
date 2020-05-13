using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Uno.CustomProviders;
using Uno.Data;
using Uno.Entities;

namespace Uno
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

          

            using (var scope = host.Services.CreateScope())
            {
                //contenitore di inserimento delle dipendenze:
                var services = scope.ServiceProvider;
                try
                {
                    //var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    //Ottenere un'istanza del contesto di database dal contenitore di inserimento delle dipendenze
                    var context = services.GetRequiredService<ApplicationDbContext>();
                    //inizializzare i dati la prima volta che viene creato il db
                    //con le migration
                    DbInitializer.Initialize(context, userManager);
                }
                catch (Exception ex)
                {
                    ///????
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
             .ConfigureLogging(l =>
             {
                 // l.AddConsole(options => options.IncludeScopes = true);
                 //l.AddEventSourceLogger();
                 //l.AddTraceSource("mySwitch");
                 //aggiungo il log personalizzato per la scrittura nel db dell'applicativo
                 
                 l.AddDb();
                 l.AddFilter((provider, category, logLevel) =>
                 {
                     // Solo i nostri eventi informativi
                     //if (logLevel == LogLevel.Information && category.StartsWith("Uno"))
                     //if (category.StartsWith("Uno.Controllers.HomeController")) //filtra solo gli eventi inerenti l'applicazione
                     if (category == "Uno.Controllers.HomeController") //filtra solo gli eventi inerenti l'applicazione
                     {
                         return true;
                     }

                     return false;
                 });
                 //.AddFilter((provider, category, logLevel) =>
                 //{
                 //    // Solo i nostri eventi informativi
                 //    //if (logLevel == LogLevel.Information && category.StartsWith("Uno"))
                 //    //if (category.StartsWith("Uno.Controllers.HomeController")) //filtra solo gli eventi inerenti l'applicazione
                 //    if (category == "Uno.Controllers.HomeController") //filtra solo gli eventi inerenti l'applicazione
                 //    {
                 //        return true;
                 //    }

                 //    return false;
                 //});
                 //l.AddFilter<AppDbLoggerProvider>("Uno.Controllers.HomeController", LogLevel.Information);

             })
                .UseStartup<Startup>();
    }
}
