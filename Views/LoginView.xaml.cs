using System;
using DriverApp.Models;
using DriverApp.ViewModels;
using Microsoft.Maui.Controls;
// using DriverApp.ViewModels; // Commented out because the namespace does not exist or is incorrect

namespace DriverApp.Views
{
    public partial class LoginView : ContentPage
    {
        public LoginView()
        {
            InitializeComponent();
            BindingContext = new LoginViewModel();

        }
        // This method is called automatically when navigating with Shell parameters
        // public void ApplyQueryAttributes(IDictionary<string, object> query)
        // {
        //     if (query.TryGetValue("LoginRequest", out var value) && value is LoginRequest request)
        //     {
        //         if (BindingContext is LoginViewModel vm)
        //         {
        //             // Optionally pass the LoginRequest to your ViewModel
        //             // vm.InitializeWithRequest(request);
        //         }
        //     }
        // }
    }
}
