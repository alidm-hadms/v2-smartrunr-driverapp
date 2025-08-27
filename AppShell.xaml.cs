using DriverApp.Views;

namespace DriverApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		// 🔹 Explicitly register critical routes
		Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
		Routing.RegisterRoute(nameof(OtpView), typeof(OtpView));
		
		Routing.RegisterRoute(nameof(DashboardView), typeof(DashboardView));
		Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
		Routing.RegisterRoute(nameof(EarningsView), typeof(EarningsView));
		Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
		
		Routing.RegisterRoute(nameof(WalletView), typeof(WalletView));


		//Helpers.NavigationHelper.RegisterAllRoutes(); // ✅ auto-register all pages - 14-08-25 at 23:46

		//RegisterRoute<LoginView>();
		//RegisterRoute<DashboardPage>();

	}
	// private void RegisterRoute<TPage>() where TPage : Page
	// {
	// 	var routeName = typeof(TPage).Name;
	// 	Routing.RegisterRoute(routeName, typeof(TPage));
	// }
}
