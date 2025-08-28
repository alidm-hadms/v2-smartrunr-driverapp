using DriverApp.ViewModels;

namespace DriverApp.Views
{
    public partial class TopUpPage : ContentPage
    {
        private TopUpPageViewModel ViewModel => BindingContext as TopUpPageViewModel;

        public TopUpPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (ViewModel != null)
            {
                ViewModel.LoadRechargeOptionsCommand.Execute(null);
                _ = ViewModel.LoadWalletBalanceAsync();
            }
        }
    }
}
