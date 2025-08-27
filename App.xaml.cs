using DriverApp.Helpers;

namespace DriverApp;

public partial class App : Application
{
	public App()
	{

		InitializeComponent();
		//NavigationHelper.RegisterAllRoutes(); // 27-08-2025
    	MainPage = new AppShell(); // ensure Shell is the root // 27-08-2025
	}
	// Enable below code if MainPage = new AppShell(); commented above
	// protected override Window CreateWindow(IActivationState? activationState)
	// {
	// 	return new Window(new AppShell());
	// }
}