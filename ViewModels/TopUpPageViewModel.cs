using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DriverApp.Helpers;
using DriverApp.Models;
using DriverApp.Views;
using Microsoft.Maui.Controls;

namespace DriverApp.ViewModels
{
    public class TopUpPageViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ApiClient _apiClient;

        public TopUpPageViewModel()
        {
            _apiClient = new ApiClient(ApiConfig.BaseUrl);
            LoadRechargeOptionsCommand = new Command(async () => await LoadRechargeOptionsAsync());
            PayCommand = new Command(async () => await PayWithRazorpay());
        }

        private ObservableCollection<RechargeOption> _rechargeOptions = new();
        public ObservableCollection<RechargeOption> RechargeOptions
        {
            get => _rechargeOptions;
            set { _rechargeOptions = value; OnPropertyChanged(); }
        }

        private RechargeOption _selectedOption;
        public RechargeOption SelectedOption
        {
            get => _selectedOption;
            set { _selectedOption = value; OnPropertyChanged(); }
        }

        private decimal _walletBalance;
        public decimal WalletBalance
        {
            get => _walletBalance;
            set { _walletBalance = value; OnPropertyChanged(); }
        }

        public ICommand LoadRechargeOptionsCommand { get; }
        public ICommand PayCommand { get; }

        public async Task LoadRechargeOptionsAsync()
        {
            try
            {
                var response = await _apiClient.GetAsync<RechargeResponse>(ApiConstant.Wallet.TopUpOptions);

                if (response != null && response.status?.ToLower() == "success")
                {
                    RechargeOptions.Clear();
                    foreach (var r in response.rechargedetails)
                    {
                        RechargeOptions.Add(new RechargeOption
                        {
                            RechargeAmount = r.rechargeamount,
                            AdditionalInformation = r.additionalinformation
                        });
                    }
                }

                await RefreshWalletBalance();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task RefreshWalletBalance()
        {
            try
            {
                var response = await _apiClient.GetAsync<WalletBalanceResponse>(ApiConstant.Wallet.AvailableWalletBalance);
                if (response != null && response.status?.ToLower() == "success")
                {
                    WalletBalance = response.balance;
                }
            }
            catch { /* ignore */ }
        }

        private async Task PayWithRazorpay()
        {
            if (SelectedOption == null)
            {
                await Application.Current.MainPage.DisplayAlert("Select Amount", "Please select a recharge amount.", "OK");
                return;
            }

            try
            {

                string driverMobile = AppState.currentDriverMobileNumber!;// TODO: bind actual driver number
                double amount = Convert.ToDouble(SelectedOption.RechargeAmount);

                // Step 1: Create Order
                var orderResponse = await _apiClient.GetAsync<CreateOrderResponse>(
                    $"{ApiConstant.Wallet.CreateRazorPayOrderId}?drivermobilenumber={driverMobile}&amount={amount}");

                if (orderResponse == null || orderResponse.status?.ToLower() != "success")
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to create order id.", "OK");
                    return;
                }

                string orderId = orderResponse.order_id;
                string razorpayKey = "rzp_live_InPyeDiD1O69Sq"; // TODO: replace with live key

                // Step 2: Open Razorpay Page
                var razorpayPage = new RazorpayPage(orderId, amount, driverMobile, razorpayKey);

                razorpayPage.PaymentSuccess += async (paymentId, returnedOrderId, signature) =>
                {
                    // Step 3: Validate Signature
                    var validateReq = new ValidateSignatureRequest
                    {
                        drivermobilenumber = driverMobile,
                        order_id = returnedOrderId,
                        razorpaypaymentid = paymentId,
                        amount = amount,
                        razorpaysignature = signature
                    };

                    var validationResponse = await _apiClient.PostAsync<ValidateSignatureRequest, ValidateSignatureResponse>(
                        ApiConstant.Wallet.ValidateRazorPaySignature, validateReq);

                    if (validationResponse != null && validationResponse.status?.ToLower() == "success")
                    {
                        await Application.Current.MainPage.DisplayAlert("Success", "Payment validated successfully!", "OK");
                        await RefreshWalletBalance();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Payment validation failed!", "OK");
                    }
                };

                await Application.Current.MainPage.Navigation.PushModalAsync(razorpayPage);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }


        public async Task LoadWalletBalanceAsync()
        {
            try
            {
                string mobileNumber = AppState.currentDriverMobileNumber!;
                // 1. Wallet Balance
                var walletRequest = new WalletRequest { driverMobileNumber = mobileNumber };
                var walletResponse = await _apiClient.PostAsync<WalletRequest, WalletResponse>(
                    ApiConstant.Wallet.AvailableWalletBalance, walletRequest);

                if (walletResponse != null && walletResponse.status?.ToLower() == "success")
                {
                    WalletBalance = walletResponse.walletbalance;
                }
                else
                {
                    WalletBalance = 0;
                }
                
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }




        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
