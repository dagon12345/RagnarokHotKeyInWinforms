using ApplicationLayer.ChildForms;
using ApplicationLayer.Dto;
using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using ApplicationLayer.Service.RagnarokService;
using ApplicationLayer.Utilities;
using ApplicationLayer.Validator;
using Domain.Constants;
using Domain.Security;
using FluentValidation;
using Infrastructure;
using Infrastructure.Factory;
using Infrastructure.Repositories.Interface;
using Infrastructure.Repositories.Service;
using Infrastructure.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RagnarokHotKeyInWinforms.RagnarokHotKeyInWinforms;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
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
            #region Updater
            //NOTE: This will differ in app the application itself will compare even if it's the same constant. When updated the installed app can detect
            string currentVersion = GlobalConstants.Version;
            string versionUrl = GlobalConstants.Version;
            // 🌐 GitHub URLs
            string msiUrl = GlobalConstants.MsiUrl;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string latestVersion = versionUrl.Trim();

                    if (latestVersion != currentVersion)
                    {
                        DialogResult result = MessageBox.Show(
                            $"A new version ({latestVersion}) is available. Do you want to update?",
                            "Ferocity Update",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Information
                        );

                        if (result == DialogResult.Yes)
                        {
                            if (File.Exists(@".\FerocityInstaller.msi")) { File.Delete(@".\FerocityInstaller.msi"); }
                            string msiPath = @".\FerocityInstaller.msi";
                            string extractPath = @".\";
                            client.DownloadFile(msiUrl, msiPath);
                            Directory.CreateDirectory(extractPath);
                            //Start the process
                            Process process = new Process();
                            process.StartInfo.FileName = "msiexec";
                            process.StartInfo.Arguments = String.Format("/i FerocityInstaller.msi");
                            process.Start();
                            Application.Exit(); // Gracefully exit current app
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update check failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            #endregion
            AppConfig.LoadConfig();// for supported_server address
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
                    var userSetting = ServiceProvider.GetRequiredService<IUserSettingService>();
                    var baseTable = ServiceProvider.GetRequiredService<IBaseTableService>();

                    //Elevation request
                    //if (!ElevationHelper.EnsureElevated())
                    //    return;

                    // Run the form
                    var signIn = new SignInForm(getUserInfo, userSignIn, userCredentials, loginService, password, userSetting, baseTable);
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
                    services.AddTransient<StatusRecoveryForm>();
                    services.AddTransient<AutopotForm>();
                    services.AddTransient<SkillSpammerForm>();
                    services.AddTransient<AttackDefendModeForm>();
                    services.AddTransient<AutoBuffStuffsForm>();
                    services.AddTransient<AutoBuffSkillsForm>();
                    services.AddTransient<MacroSongsForm>();
                    services.AddTransient<MacroSwitchForm>();
                    services.AddSingleton<SubjectService>(); // Since Subject manages observer list, singleton is ideal
                    #endregion

                    #region Infrastructure Layer Services
                    services.AddTransient<IBaseTableRepository, BaseTableRepository>();
                    services.AddTransient<IStoredCredentialRepository, StoredCredentialRepository>();
                    services.AddTransient<IUserSettingRepository, UserSettingRepository>();
                    services.AddSingleton<IHasher, Pbkdf2Hasher>();
                    services.AddSingleton(typeof(IDbContextFactory<>), typeof(DbContextFactory<>));

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
