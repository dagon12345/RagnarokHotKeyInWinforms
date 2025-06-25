using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using Infrastructure;
using Infrastructure.Repositories.Interface;
using Infrastructure.Repositories.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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

            AppConfig.Load();
            using (var tunnel = new SshTunnelManager(AppConfig.SshSettings, AppConfig.DatabaseSettings))
            {
                tunnel.Start();

                var connStr = $"Server=127.0.0.1;Port={AppConfig.DatabaseSettings.LocalPort};Database={AppConfig.DatabaseSettings.Name};" +
                      $"Uid={AppConfig.DatabaseSettings.User};Pwd={AppConfig.DatabaseSettings.Password};";

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                // Create the host
                var host = CreateHostBuilder(connStr).Build();
                ServiceProvider = host.Services;

                // Resolve the IGetUserInfo service
                var getUserInfo = ServiceProvider.GetRequiredService<IGetUserInfo>();
                var userSignIn = ServiceProvider.GetRequiredService<ISignIn>();
                var userCredentials = ServiceProvider.GetRequiredService<IStoredCredentialService>();
                // Run the form
                Application.Run(new SignInForm(getUserInfo, userSignIn, userCredentials));

            }



        }
        static IHostBuilder CreateHostBuilder(string connectionString)
        {
            var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString,
                    mysqlOptions => { }));




                    #region Application Layer Services
                    services.AddScoped<IGetUserInfo, GetUserInfoService>();
                    services.AddScoped<ISignIn, SignInService>();
                    services.AddScoped<IStoredCredentialService, StoredCredentialService>();
                    services.AddScoped<IUserSettingService, UserSettingService>();
                    services.AddScoped<IBaseTableService, BaseTableService>();
                    #endregion

                    #region Infrastructure Layer Services
                    services.AddScoped<IBaseTableRepository, BaseTableRepository>();
                    services.AddScoped<IStoredCredentialRepository, StoredCredentialRepository>();
                    services.AddScoped<IUserSettingRepository, UserSettingRepository>();
                    #endregion
                });
        }


    }
}
