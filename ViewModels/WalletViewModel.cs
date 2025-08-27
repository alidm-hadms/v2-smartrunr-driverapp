using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using DriverApp.Helpers;
using DriverApp.Models;
using Microsoft.Maui.Controls;

namespace DriverApp.ViewModels
{
    public class WalletViewModel : BaseViewModel, INotifyPropertyChanged
    {
        private readonly ApiClient _apiClient;

        public WalletViewModel()
        {
            _apiClient = new ApiClient(ApiConfig.BaseUrl);// Initialize ApiClient
            LoadDataCommand = new Command(async () => await LoadDataAsync());
            NavigateToTopUpCommand = new Command(async () => await NavigateToTopUpPage());
        }

        private decimal _balance;
        public decimal Balance
        {
            get => _balance;
            set { _balance = value; OnPropertyChanged(); }
        }

        private bool _showAlert;
        public bool ShowAlert
        {
            get => _showAlert;
            set { _showAlert = value; OnPropertyChanged(); }
        }

        private string _alertMessage;
        public string AlertMessage
        {
            get => _alertMessage;
            set { _alertMessage = value; OnPropertyChanged(); }
        }

        private ObservableCollection<TopUpTransaction> _transactions = new();
        public ObservableCollection<TopUpTransaction> Transactions
        {
            get => _transactions;
            set { _transactions = value; OnPropertyChanged(); }
        }

        public ICommand LoadDataCommand { get; }
        public ICommand NavigateToTopUpCommand { get; }

        public async Task LoadDataAsync()
        {
             ShowLoader();
            try
            {
                #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                string mobileNumber = AppState.currentDriverMobileNumber;
                #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                #pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                string driverMobileNumber = AppState.currentDriverMobileNumber;
                #pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                if (string.IsNullOrEmpty(driverMobileNumber))
                {
                    // AlertMessage = "Driver mobile number is not set.";
                    // ShowAlert = true;
                    // return;
                    await Application.Current.MainPage.DisplayAlert("", "Login Expired. Please login.", "OK");
                }

                // 1. Wallet Balance
                var walletRequest = new WalletRequest { driverMobileNumber = mobileNumber };
                var walletResponse = await _apiClient.PostAsync<WalletRequest, WalletResponse>(
                    ApiConstant.Wallet.AvailableWalletBalance, walletRequest);

                if (walletResponse != null && walletResponse.status?.ToLower() == "success")
                {
                    Balance = walletResponse.walletbalance;
                }
                else
                {
                    Balance = 0;
                }

                // 2. Top-up History
                var topUpResponse = await _apiClient.PostAsync<WalletRequest, TopUpResponse>(
                    ApiConstant.Wallet.TopUpTransactionHistory, walletRequest);

                if (topUpResponse != null && topUpResponse.status?.ToLower() == "success")
                {
                    Transactions.Clear();
                    foreach (var t in topUpResponse.topupdetails)
                    {
                        Transactions.Add(new TopUpTransaction
                        {
                            Date = t.date,
                            PaymentId = t.paymentid,
                            TopUpAmount = t.topupamout,
                            AdditionalAmount = t.additionalamout,
                            TotalAmount = t.totalamount
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                 HideLoader();
            }
        }

        private async Task NavigateToTopUpPage()
        {
            // Replace with actual navigation to Top-Up page
            await NavigationHelper.NavigateToDashboard();// Navigation chage
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // API request/response models
    public class WalletRequest
    {
        public string? driverMobileNumber { get; set; }
    }

    public class WalletResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public decimal walletbalance { get; set; }
    }

    public class TopUpResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public List<GetTopUpDetailsFromApi> topupdetails { get; set; }
    }

    public class GetTopUpDetailsFromApi
    {
        public string? date { get; set; }
        public string? paymentid { get; set; }
        public decimal topupamout { get; set; }
        public decimal additionalamout { get; set; }
        public decimal totalamount { get; set; }
    }

    // Model for binding
    public class TopUpTransaction
    {
        public string? Date { get; set; }
        public string? PaymentId { get; set; }
        public decimal TopUpAmount { get; set; }
        public decimal AdditionalAmount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
