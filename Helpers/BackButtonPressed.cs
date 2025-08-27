using Microsoft.Maui.Controls;
using DriverApp.Helpers;

namespace DriverApp.Helpers
{
    public class BackButtonPressed : ContentPage
    {
        protected override bool OnBackButtonPressed()
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await NavigationHelper.NavigateToBack();
            });

            return true; // ✅ cancel default back, use our logic
        }
    }
}
