using Autofac;
using System.Windows;
using AutofacDependence;
using Services.Interfaces;
using User.Model;
using User.View;
using User.ViewModel;

namespace User
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void _Startup(object sender, StartupEventArgs e)
        {
            var builderBase = new ContainerBuilder();
            builderBase.RegisterModule(new RepositoryModule());
            builderBase.RegisterModule(new ServicesModule());
            var containerBase = builderBase.Build();

            var viewBase = new UserWindow(){DataContext = new UserViewModel(
                containerBase.Resolve<ITasksService>(),
                containerBase.Resolve<IMethodService>())};
            viewBase.Show();
        }
    }
   
}
