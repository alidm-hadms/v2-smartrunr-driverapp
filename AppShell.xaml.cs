using DriverApp.Views;

namespace DriverApp;

public partial class AppShell : Shell
{
	public AppShell()
	{
		Helpers.NavigationHelper.RegisterAllRoutes(); // ✅ auto-register all pages - 14-08-25 at 23:46
		InitializeComponent();

		// Register route for LoginView
		//Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
		//Routing.RegisterRoute(nameof(OtpView), typeof(OtpView));

		//RegisterRoute<LoginView>();
		//RegisterRoute<DashboardPage>();

	}
	// private void RegisterRoute<TPage>() where TPage : Page
	// {
	// 	var routeName = typeof(TPage).Name;
	// 	Routing.RegisterRoute(routeName, typeof(TPage));
	// }
}
