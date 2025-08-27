using DriverApp.ViewModels;
using Microsoft.Maui.Controls;

namespace DriverApp.Views
{
    public partial class WalletView : ContentView
    {
        public WalletView()
        {
            InitializeComponent();

            var vm = BindingContext as WalletViewModel;
            vm?.LoadDataCommand.Execute(null);
        }
    }
}
