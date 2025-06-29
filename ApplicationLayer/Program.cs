using ApplicationLayer.ChildForms;
using ApplicationLayer.Dto;
using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using ApplicationLayer.Utilities;
using ApplicationLayer.Validator;
using Domain.Security;
using FluentValidation;
using Infrastructure;
using Infrastructure.Repositories.Interface;
using Infrastructure.Repositories.Service;
using Infrastructure.Service;
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
        public class LoggingAnchor { }


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
         // Properly typed ServiceProvider as IServiceProvider
        public static IServiceProvider ServiceProvider { get; private set; }
        [STAThread]
        static void Main()
        {
            bool alreadyRunning = false;
            using (var mutex = new System.Threading.Mutex(true, "MyUniqueAppNameMutex", out alreadyRunning))
            {
                if (!alreadyRunning)
                {
                    MessageBox.Show("The program is already running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                AppConfig.Load();
                using (var tunnel = new SshTunnelManager(AppConfig.SshSettings, AppConfig.DatabaseSettings))
                {
                    try
                    {
                        tunnel.Start();
                    }
                    catch (Exception ex)
                    {
                        LoggerService.LogError(ex, "Failed to start SSH tunnel.");
                        MessageBox.Show("Unable to establish connection. Another instance may be running.", "Startup Error");
                        return;
                    }


                    var connStr = $"Server=127.0.0.1;Port={AppConfig.DatabaseSettings.LocalPort};Database={AppConfig.DatabaseSettings.Name};" +
                          $"Uid={AppConfig.DatabaseSettings.User};Pwd={AppConfig.DatabaseSettings.Password};";

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    // Create the host
                    var host = CreateHostBuilder(connStr).Build();
                    ServiceProvider = host.Services;

                    #region Error Handling
                    // Configure the logger globally
                    var logger = ServiceProvider.GetRequiredService<ILogger<LoggingAnchor>>();
                    LoggerService.Configure(logger);

                    // Optional: hook global exception handling
                    Application.ThreadException += (s, e) =>
                        LoggerService.LogError(e.Exception, "Unhandled UI exception");

                    AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                        LoggerService.LogError(e.ExceptionObject as Exception, "Unhandled non-UI exception");
                    #endregion

                    // Resolve the IGetUserInfo service
                    var getUserInfo = ServiceProvider.GetRequiredService<IGetUserInfo>();
                    var userSignIn = ServiceProvider.GetRequiredService<ISignIn>();
                    var userCredentials = ServiceProvider.GetRequiredService<IStoredCredentialService>();
                    var loginService = Program.ServiceProvider.GetRequiredService<LoginService>();
                    var password = Program.ServiceProvider.GetRequiredService<PasswordRecoveryService>();

                    // Run the form
                    var signIn = new SignInForm(getUserInfo, userSignIn, userCredentials, loginService, password);
                    FormManager.SignInInstance = signIn;
                    Application.Run(signIn);

                }
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
                    services.AddTransient<IGetUserInfo, GetUserInfoService>();
                    services.AddTransient<ISignIn, SignInService>();
                    services.AddTransient<IStoredCredentialService, StoredCredentialService>();
                    services.AddTransient<IUserSettingService, UserSettingService>();
                    services.AddTransient<IBaseTableService, BaseTableService>();
                    services.AddTransient<IEmailService, SmtpEmailService>();
                    services.AddTransient<IRegistrationService, RegistrationService>();
                    services.AddTransient<LoginService>();
                    services.AddTransient<PasswordRecoveryService>();
                    services.AddTransient<ToggleApplicationForm>();

                    #endregion

                    #region Infrastructure Layer Services
                    services.AddTransient<IBaseTableRepository, BaseTableRepository>();
                    services.AddTransient<IStoredCredentialRepository, StoredCredentialRepository>();
                    services.AddTransient<IUserSettingRepository, UserSettingRepository>();
                    services.AddSingleton<IHasher, Pbkdf2Hasher>();
                    #endregion

                    #region Validator
                    services.AddSingleton<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
                    services.AddSingleton<IValidator<SignInRegistrationDto>, SignInRegistrationDtoValidator>();
                    services.AddSingleton<IValidator<LoginDto>, LoginDtoValidator>();
                    services.AddSingleton<IValidator<EmailDto>, EmailDtoValidator>();
                    #endregion
                });
        }


    }
}
