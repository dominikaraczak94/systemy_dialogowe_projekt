using System.Windows;
using Caliburn.Micro;
using WashingMachineServiceWPF.ViewModels;

namespace WashingMachineServiceWPF
{
    public class AppBootstrapper : BootstrapperBase
    {
        public AppBootstrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<WashingMachineViewModel>();
        }
    }
}
