using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Maui.Controls;
using DriverApp.Views; // Use Xamarin.Forms if not on MAUI

namespace DriverApp.Helpers
{
    public static class NavigationHelper
    {
        private static bool _routesRegistered = false;

        /// <summary>
        /// Registers all Page types in all loaded assemblies for Shell navigation.
        /// Call this once during app startup (e.g., in App.xaml.cs).
        /// </summary>
        public static void RegisterAllRoutes()
        {
            if (_routesRegistered)
                return;

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var pageTypes = assemblies
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch (TypeLoadException)
                    {
                        // Return empty if some types can't be loaded
                        return Array.Empty<Type>();
                    }
                    catch (Exception ex)
                    {
                        // Handle ReflectionTypeLoadException dynamically to avoid hard dependency
                        if (ex.GetType().Name == "ReflectionTypeLoadException")
                        {
                            var typesProp = ex.GetType().GetProperty("Types");
                            if (typesProp?.GetValue(ex) is Type[] types)
                                return types.Where(t => t != null)!;
                        }
                        return Array.Empty<Type>();
                    }
                })
                .Where(t => t != null && t.IsSubclassOf(typeof(Page)) && !t.IsAbstract);

            foreach (var pageType in pageTypes)
            {
                var route = pageType.Name; // e.g., "LoginView"
                Routing.RegisterRoute(route, pageType);
            }
            _routesRegistered = true;
        }

        public static async Task NavigateToLoginView()
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}"); // With out back navigation
        }
        public static async Task NavigateToOtpView()
        {
            //await Shell.Current.GoToAsync($"//{nameof(OtpView)}"); // With out back navigation
            await Shell.Current.GoToAsync(nameof(OtpView));
        }
        // public static async Task NavigateToDashboard()
        // {
        //     await Shell.Current.GoToAsync($"//{nameof(DashboardView)}"); 
        // }
        public static async Task NavigateToDashboard()
        {
            // If Shell is available, try using the route
            if (Shell.Current != null)
            {
                try
                {
                    await Shell.Current.GoToAsync(nameof(DashboardView));
                }
                catch (Exception ex)
                {
                    // Log and fallback
                    System.Diagnostics.Debug.WriteLine($"Shell navigation failed: {ex.Message}");
                }
            }
            // Fallback (or if Shell is not in use): 
            // Replace MainPage entirely so DashboardView is root, no back navigation possible
            Application.Current.MainPage = new NavigationPage(new DashboardView());
        }

        public static async Task NavigateToOrderDetails(string bookingRef)
        {
            if (Shell.Current != null)
            {
                await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?bookingRefNumber={bookingRef}");
            }
            else
            {
                Application.Current.MainPage = new NavigationPage(new OrderDetailsPage(bookingRef));
            }
            
        }


            
    }
}
