namespace DriverApp.Views;

public partial class DashboardView : ContentPage
{
    public DashboardView()
    {
        InitializeComponent();
        ShowTab("Home"); // Default tab
    }

    private void OnHomeTapped(object sender, EventArgs e) => ShowTab("Home");

    private void OnInProgressTapped(object sender, EventArgs e) => ShowTab("In Progress");
    private void OnEarningsTapped(object sender, EventArgs e) => ShowTab("Earnings");
    private void OnWalletTapped(object sender, EventArgs e) => ShowTab("Wallet");
    private void OnAccountTapped(object sender, EventArgs e) => ShowTab("Account");

    // private void ShowTab(string tabName)
    // {
    //     // Reset all indicators
    //     HomeIndicator.BackgroundColor = EarningsIndicator.BackgroundColor =
    //         WalletIndicator.BackgroundColor = AccountIndicator.BackgroundColor = Colors.Transparent;

    // Swap content based on tab
    // switch (tabName)
    // {
    //     case "Home":
    //         HomeIndicator.BackgroundColor = Colors.Blue;
    //         MainContent.Content = new HomePage();
    //         break;

    //     case "In Progress":
    //         HomeIndicator.BackgroundColor = Colors.Blue;
    //         MainContent.Content = new HomePage();
    //         break;

    //     case "Earnings":
    //         EarningsIndicator.BackgroundColor = Colors.Blue;
    //         MainContent.Content = new EarningsView(); // <-- Load your EarningsView.xaml here
    //         //MainContent.Content = new Label { Text = "Earnings Content", FontSize = 18, HorizontalOptions = LayoutOptions.Center };
    //         break;

    //     case "Wallet":
    //         WalletIndicator.BackgroundColor = Colors.Blue;
    //         MainContent.Content = new WalletView();
    //         break;

    //     case "Account":
    //         AccountIndicator.BackgroundColor = Colors.Blue;
    //         MainContent.Content = new HomePage();
    //         break;
    // }
    //}
    
    private async void ShowTab(string tabName)
    {
        HomeIndicator.BackgroundColor = EarningsIndicator.BackgroundColor =
            WalletIndicator.BackgroundColor = AccountIndicator.BackgroundColor = Colors.Transparent;

        switch (tabName)
        {
            case "Home":
                HomeIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new HomePage(); // ContentView works here
                break;

            case "In Progress":
                EarningsIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new HomePage();
                break;

            case "Earnings":
                EarningsIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new EarningsView();
                break;

            case "Wallet":
                WalletIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new WalletView(); // ContentView
                break;

            case "Account":
                EarningsIndicator.BackgroundColor = Colors.Blue;
                MainContent.Content = new HomePage();
                break;
        }
    }

}
