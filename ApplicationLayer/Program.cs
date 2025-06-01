using ApplicationLayer.Forms;
using ApplicationLayer.Interface;
using ApplicationLayer.Service;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

        

            // Create the host
            var host = CreateHostBuilder().Build();
            ServiceProvider = host.Services;
            // Resolve the IGetUserInfo service
            var getUserInfo = ServiceProvider.GetRequiredService<IGetUserInfo>();

            // Run the Sample form
            Application.Run(new SignInForm(getUserInfo));
            
        }

        static IHostBuilder CreateHostBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    /*AddTransient
                        Transient lifetime services are created each time they are requested. This lifetime works best for lightweight, stateless services.
 
                        AddScoped
                        Scoped lifetime services are created once per request.
                         
                        AddSingleton
                        Singleton lifetime services are created the first time they are requested (or when ConfigureServices is run if you specify an instance there) and then every subsequent request will use the same instance.*/
                    services.AddScoped<IGetUserInfo, GetUserInfoService>();
           
                });
        }

    }
}
