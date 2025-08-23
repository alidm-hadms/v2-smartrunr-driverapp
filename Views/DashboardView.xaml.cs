namespace DriverApp.Views;

public partial class DashboardView : ContentPage
{
    public DashboardView()
    {
        InitializeComponent();
        ShowTab("Home"); // Default tab
    }

    private void OnHomeTapped(object sender, EventArgs e) => ShowTab("Home");
    private void OnEarningsTapped(object sender, EventArgs e) => ShowTab("Earnings");
    private void OnWalletTapped(object sender, EventArgs e) => ShowTab("Wallet");
    private void OnAccountTapped(object sender, EventArgs e) => ShowTab("Account");

    private void ShowTab(string tabName)
    {
        // Reset all indicators
        HomeIndicator.BackgroundColor = EarningsIndicator.BackgroundColor =
            WalletIndicator.BackgroundColor = AccountIndicator.BackgroundColor = Colors.Transparent;

        // Swap content based on tab
        switch (tabName)
        {
            case "Home":
                HomeIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new Label { Text = "Home Content", FontSize = 18, HorizontalOptions = LayoutOptions.Center };
                break;

            case "Earnings":
                EarningsIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new EarningsView(); // <-- Load your EarningsView.xaml here
                break;

            case "Wallet":
                WalletIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new Label { Text = "Wallet Content", FontSize = 18, HorizontalOptions = LayoutOptions.Center };
                break;

            case "Account":
                AccountIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new Label { Text = "Account Content", FontSize = 18, HorizontalOptions = LayoutOptions.Center };
                break;
        }
    }
}
