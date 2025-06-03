using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using Infrastructure;
using Infrastructure.Repositories.Interface;
using Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Windows.Forms;

namespace RagnarokHotKeyInWinforms
{
    static class Program
    {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
         // Properly typed ServiceProvider as IServiceProvider
        public static IServiceProvider ServiceProvider { get; private set; }
        [STAThread]
        static void Main()
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

       
            // Create the host
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;

            // Get the DbContext instance from DI
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated(); // Applies pending migrations
            }

            // Resolve the IGetUserInfo service
            var getUserInfo = ServiceProvider.GetRequiredService<IGetUserInfo>();
            var userSignIn = ServiceProvider.GetRequiredService<ISignIn>();
            var userCredentials = ServiceProvider.GetRequiredService<IStoredCredentialService>();
            // Run the form
            Application.Run(new SignInForm(getUserInfo, userSignIn, userCredentials));

        }
        static IHostBuilder CreateHostBuilder()
        {
            var loggerfactory = LoggerFactory.Create(builder => builder.AddConsole());
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    // Register ApplicationDbContext using DI
                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseLoggerFactory(loggerfactory)
                        .UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                    /*AddTransient
                        Transient lifetime services are created each time they are requested. This lifetime works best for lightweight, stateless services.
 
                        AddScoped
                        Scoped lifetime services are created once per request.
                         
                        AddSingleton
                        Singleton lifetime services are created the first time they are requested (or when ConfigureServices is run if you specify an instance there) and then every subsequent request will use the same instance.*/
                    #region Application Layer Services
                    services.AddScoped<IGetUserInfo, GetUserInfoService>();
                    services.AddScoped<ISignIn, SignInService>();
                    services.AddScoped<IStoredCredentialService, StoredCredentialService>();
                    #endregion

                    #region Infrastructure Layer Services
                    services.AddScoped<IBaseTableRepository, BaseTableRepository>();
                    services.AddScoped<IStoredCredentialRepository, StoredCredentialRepository>();
                    #endregion

                });
        }

    }
}
