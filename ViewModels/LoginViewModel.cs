using System;
using System.ComponentModel;
using System.Windows.Input;
using System.Threading.Tasks;
using DriverApp.Models;
using DriverApp.Views;
using DriverApp.Helpers;

namespace DriverApp.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private string _mobileNumber = string.Empty;
        private readonly ApiClient _apiClient;
        public string driverMobileNumber
        {
            get => _mobileNumber;
            set
            {
                if (_mobileNumber != value)
                {
                    _mobileNumber = value;
                    OnPropertyChanged(nameof(driverMobileNumber));
                }
            }
        }

        public ICommand LoginCommand { get; }
        public LoginViewModel()
        {
            // ✅ Use base URL from ApiConfig, not an endpoint
            _apiClient = new ApiClient(ApiConfig.BaseUrl);
            LoginCommand = new Command(
                async () => await GenerateOtp(),
                () => !IsBusy
            );
        }

        public async Task GenerateOtp()
        {
            var mainPage = App.Current?.MainPage;

            string mobileNumber = driverMobileNumber?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(mobileNumber))
            {
                // ✅ Use DisplayAlert for better user feedback
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Invalid Number",
                                "Please enter a valid 10-digit mobile number.", "OK");
                }
                                
                return;
            }

            ShowLoader();

            try
            {
                var deviceToken = "DEVICE_TOKEN_123"; // TODO: Replace with actual token retrieval
                var request = new LoginRequest
                {
                    driverMobileNumber = mobileNumber,
                    tokenNumber = deviceToken
                };

                var response = await _apiClient.PostAsync<LoginRequest, LoginResponse>(
                    ApiConstant.Auth.Login, request);
                if (response?.status?.Equals("Success", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // if (mainPage != null)
                    //     await mainPage.DisplayAlert("Success", response.message, "OK");

                    AppState.currentDriverMobileNumber = mobileNumber;
                    //Preferences.Set("currentDriverMobileNumber", mobileNumber); // Set in preferences
                    //var mobileNumberStored = Preferences.Get("currentDriverMobileNumber", string.Empty);// Get from preferences


                    // ✅ Navigate using helper with Shell fallback

                    //await Shell.Current.GoToAsync($"//{nameof(OtpView)}"); //With out back navigation
                    //await Shell.Current.GoToAsync(nameof(OtpView)); // With back navigation
                    await NavigationHelper.NavigateToOtpView();

                    //OTP success → Navigate to Dashboard (replace stack so back won’t return to OTP):
                    //await Shell.Current.GoToAsync($"//{nameof(DashboardPage)}");

                }
                else
                {
                    if (mainPage != null)
                        await mainPage.DisplayAlert("Login Failed", response?.message ?? "Unknown error", "OK");
                }
            }
            catch (Exception ex)
            {
                var errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += "\nInner: " + ex.InnerException.Message;
                }

                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", errorMessage, "OK");
                }
            }
            
            finally
            {
                HideLoader();
            }
        }
       
    }
}
