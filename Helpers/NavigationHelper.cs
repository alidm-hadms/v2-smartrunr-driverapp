using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Maui.Controls;
using DriverApp.Views; // Use Xamarin.Forms if not on MAUI

namespace DriverApp.Helpers
{
    public static class NavigationHelper
    {
        //private static bool _routesRegistered = false;

        /// <summary>
        /// Registers all Page types in all loaded assemblies for Shell navigation.
        /// Call this once during app startup (e.g., in App.xaml.cs).
        /// </summary>
        /// Commented all below
        // public static void RegisterAllRoutes()
        // {
        //     if (_routesRegistered)
        //         return;

        //     var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        //     var pageTypes = assemblies
        //         .SelectMany(a =>
        //         {
        //             try
        //             {
        //                 return a.GetTypes();
        //             }
        //             catch (TypeLoadException)
        //             {
        //                 // Return empty if some types can't be loaded
        //                 return Array.Empty<Type>();
        //             }
        //             catch (Exception ex)
        //             {
        //                 // Handle ReflectionTypeLoadException dynamically to avoid hard dependency
        //                 if (ex.GetType().Name == "ReflectionTypeLoadException")
        //                 {
        //                     var typesProp = ex.GetType().GetProperty("Types");
        //                     if (typesProp?.GetValue(ex) is Type[] types)
        //                         return types.Where(t => t != null)!;
        //                 }
        //                 return Array.Empty<Type>();
        //             }
        //         })
        //         .Where(t => t != null && t.IsSubclassOf(typeof(Page)) && !t.IsAbstract);

        //     foreach (var pageType in pageTypes)
        //     {
        //         var route = pageType.Name; // e.g., "LoginView"
        //         Routing.RegisterRoute(route, pageType);
        //     }
        //     _routesRegistered = true;
        // }

        // public static async Task NavigateToLoginView()
        // {
        //     //await Shell.Current.GoToAsync($"//{nameof(LoginView)}"); // With out back navigation
        //     if (Shell.Current != null)
        //     {
        //         // Navigate to LoginView as root (no back stack) 
        //         await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        //     }
        //     else
        //     {
        //         System.Diagnostics.Debug.WriteLine("⚠️ Shell.Current is null - check if AppShell is MainPage");
        //     }
        // }
        // public static async Task NavigateToOtpView()
        // {
        //     //await Shell.Current.GoToAsync($"//{nameof(OtpView)}"); // With out back navigation
        //     await Shell.Current.GoToAsync(nameof(OtpView));
        // }
        // public static async Task NavigateToDashboard()
        // {
        //     if (Shell.Current != null)
        //     {
        //         // Navigate to Dashboard as root (no back stack)
        //         await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
        //         //await Shell.Current.GoToAsync(nameof(DashboardView));
        //     }
        //     else
        //     {
        //         System.Diagnostics.Debug.WriteLine("⚠️ Shell.Current is null - check if AppShell is MainPage");
        //     }
        //             // Fallback (or if Shell is not in use): 
        //     // Replace MainPage entirely so DashboardView is root, no back navigation possible
        //     //Application.Current.MainPage = new NavigationPage(new DashboardView());
        // }

        // public static async Task NavigateToOrderDetails(string bookingRef)
        // {
        //     if (Shell.Current != null)
        //     {
        //         await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?bookingRefNumber={bookingRef}");
        //     }
        //     else
        //     {
        //         System.Diagnostics.Debug.WriteLine("⚠️ Shell.Current is null - check if AppShell is MainPage");
        //     }

        // }

        // public static async Task NavigateToBack()
        // {
        //     // // else no-op, can't go back
        //     if (Shell.Current != null)
        //     {
        //         await Shell.Current.GoToAsync("..");
        //     }
        //     else
        //     {
        //         if (Application.Current.MainPage is NavigationPage navPage && navPage.Navigation.NavigationStack.Count > 1)
        //         {
        //             await navPage.PopAsync();
        //         }
        //         else
        //         {
        //             await NavigateToDashboard(); // Navigate to Dashboard if can't go back
        //         }
        //     }
        //     // else no-op, can't go back
        // }



        public static async Task NavigateToLoginView()
        {
            // ✅ make LoginView the root (no back)
            await Shell.Current.GoToAsync($"//{nameof(LoginView)}");
        }

        public static async Task NavigateToOtpView()
        {
            // ✅ normal forward navigation, allows back to Login
            await Shell.Current.GoToAsync(nameof(OtpView));
        }

        public static async Task NavigateToDashboard()
        {
            // ✅ make DashboardView the root (no back button to OTP/Login)
            await Shell.Current.GoToAsync($"//{nameof(DashboardView)}");
        }

        public static async Task NavigateToOrderDetails(string bookingRef)
        {
            await Shell.Current.GoToAsync($"{nameof(OrderDetailsPage)}?bookingRefNumber={bookingRef}");
        }

        public static async Task NavigateToEarnings()
        {
            await Shell.Current.GoToAsync(nameof(EarningsView));
        }
        public static async Task NavigateToToUps()
        {
            await Shell.Current.GoToAsync(nameof(TopUpPage));
        }

        public static async Task NavigateToBack()
        {
            if (Shell.Current != null)
            {
                try
                {
                    await Shell.Current.GoToAsync("..");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Navigation back failed: {ex.Message}");
                }
            }
        }

    }
}