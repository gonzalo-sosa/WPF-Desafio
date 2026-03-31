using Incident.Domain.Entities;
using Incident.Infrastructure.Data;
using Incident.Infrastructure.Repositories.Implementations;
using Incident.Infrastructure.Repositories.Interfaces;
using Incident.Infrastructure.Services.Implementations;
using Incident.Infrastructure.Services.Interfaces;
using Incident.WPF;
using Incident.WPF.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;


namespace WPF_Desafio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Flujo normal de inicio

            // Inyectar dependencias e iniciar MainWindow
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DataSeeder.Initialize(context);
            }

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Settings.Default.DbString);
            });

            // Repositories
            services.AddTransient<IRepository<Ticket>, TicketRepository>();
            services.AddTransient<IRepository<User>, UserRepository>();

            // Services
            services.AddTransient<ITicketService, TicketService>();

            // ViewModels
            services.AddTransient<MainViewModel>();

            // Windows
            services.AddTransient<MainWindow>();
        }
    }
}
