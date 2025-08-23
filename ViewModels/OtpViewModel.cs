using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DriverApp.Helpers;
using DriverApp.Models;

using DriverApp.Views; // Or Microsoft.Maui.Controls if MAUI

namespace DriverApp.ViewModels
{
    public class OtpViewModel : BaseViewModel
    {
        private string _otpCode = string.Empty;
        private readonly ApiClient _apiClient;

        public string otp
        {
            get => _otpCode;
            set
            {
                if (_otpCode != value)
                {
                    _otpCode = value;
                    OnPropertyChanged(nameof(otp));
                }
            }
            //get => _otpCode;
            //set => SetProperty(ref _otpCode, value);
        }
        public ICommand VerifyOtpCommand { get; }

        public OtpViewModel()
        {
            // ✅ Use base URL from ApiConfig, not an endpoint
            _apiClient = new ApiClient(ApiConfig.BaseUrl);
            VerifyOtpCommand = new Command(async () => await VerifyOtpAsync(), () => !IsBusy);
        }

        public async Task VerifyOtpAsync()
        {
            var mainPage = App.Current?.MainPage;
            string otp = _otpCode?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(otp) || otp.Length < 4 || !int.TryParse(otp, out _))
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Invalid OTP", "Please enter a valid OTP.", "OK");
                    return;

                }
            }
            ShowLoader();

            try
            {
                var request = new
                {
                    driverMobileNumber = AppState.currentDriverMobileNumber, // store from Login step
                    otp = otp
                };

                var response = await _apiClient.PostAsync<object, LoginResponse>(
                    ApiConstant.Auth.OtpVerification, request);

                if (response?.status?.Equals("Success", StringComparison.OrdinalIgnoreCase) == true)
                {
                    if (mainPage != null)
                    {
                        await mainPage.DisplayAlert("Success", response.message, "OK");
                    }
                    // Navigate to dashboard or main page
                    //await NavigationHelper.NavigateToAsync(nameof(LoginView), new LoginView());
                    await NavigationHelper.NavigateToDashboard();
                }
                else
                {
                    if (mainPage != null)
                    {
                        await mainPage.DisplayAlert("Verification Failed", response?.message ?? "Unknown error", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                if (mainPage != null)
                {
                    await mainPage.DisplayAlert("Error", ex.Message, "OK");
                }
            }
            finally
            {
                HideLoader();
                // IsBusy = false; // Handled in BaseViewModel
            }
        }

       
    }
}
