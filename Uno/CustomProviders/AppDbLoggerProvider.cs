using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using Newtonsoft.Json;
using Uno.Data;
using Uno.Entities;

namespace Uno.CustomProviders
{
    public static class AppDbLoggerProviderExtensions
    {
        public static ILoggingBuilder AddDb(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<ILoggerProvider, AppDbLoggerProvider>();

            return builder;
        }
    }

    [ProviderAlias("AppDb")]
    public class AppDbLoggerProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, AppDbLogger> _loggers = new ConcurrentDictionary<string, AppDbLogger>();
        //private readonly ApplicationDbContext _context;
        //public AppDbLoggerProvider(ApplicationDbContext context)
        //{
        //   // _context = context;
        //}
        public ILogger CreateLogger(string categoryName)
        {
            // Caching dei logger
            return _loggers.GetOrAdd(categoryName, c => new AppDbLogger(c));
            //return _loggers.GetOrAdd(categoryName, c => new AppDbLogger(c, _context));
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }

    public class AppDbLogger : ILogger
    {
        private string _category;
      

        public AppDbLogger(string category)
        {
            _category = category;
       
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            // Accedo allo stato come coppia chiave/valore
            var values = state as IEnumerable<KeyValuePair<string, object>>;


          
            if (_category.StartsWith("Uno")) //salva solo gli eventi dell'applicazione
            {
                //string json = JsonConvert.SerializeObject(state);
                string message = formatter(state, exception);

                var builder = new ConfigurationBuilder()
                       .SetBasePath(Directory.GetCurrentDirectory())
                       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                       .AddEnvironmentVariables();
                IConfigurationRoot configuration = builder.Build();
                var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
                optionsBuilder.UseSqlServer(
                 configuration.GetConnectionString("DefaultConnection"));

                using (var context = new ApplicationDbContext(optionsBuilder.Options))
                {
                    context.Logs.Add(new Log()
                    {
                        EventID = eventId.Id,
                        LogType = logLevel.ToString(),
                        Message = message,
                        Category=_category,
                        EventTime = DateTime.Now
                    });

                    context.SaveChanges();
                }


            }
           
        }

        public bool IsEnabled(LogLevel logLevel) => true;

        public IDisposable BeginScope<TState>(TState state) => null;
    }
}