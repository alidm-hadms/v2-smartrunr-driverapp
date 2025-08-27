using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using DriverApp;
using DriverApp.Helpers;
using DriverApp.Models;
using DriverApp.Views;

public class OrderDetailsViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;

    public ICommand BackCommand { get; }

    private OrderDetailsResponse _orderDetails;
    public OrderDetailsResponse OrderDetails
    {
        get => _orderDetails;
        set
        {
            _orderDetails = value;
            OnPropertyChanged(nameof(OrderDetails));
        }
    }

    public OrderDetailsViewModel(string bookingRefNumber)
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri(ApiConfig.BaseUrl) // 👈 your base URL, e.g. "https://app.hadmservices.com/api/"
        };

        BackCommand = new Command(async () =>
        {
            //await Shell.Current.GoToAsync("..");
            await NavigationHelper.NavigateToBack();
        });



        // fire async load
        _ = LoadOrderDetails(bookingRefNumber);
    }

    private async Task LoadOrderDetails(string bookingRefNumber)
    {
        try
        {
            var request = new { bookingRefNumber };

            // full API endpoint
            string endpoint = ApiConstant.Auth.OrderSummaryDetails;

            var response = await _httpClient.PostAsJsonAsync(endpoint, request);
           
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<OrderDetailsResponse>();
                OrderDetails = result;
            }
            else
            {
                // handle error gracefully
                Console.WriteLine($"❌ API Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Exception: {ex.Message}");
        }
    }

    // 🔹 Back button handler (required for your XAML ToolbarItem)
    // private async void OnBackClicked(object sender, EventArgs e)
    // {
    //     await Shell.Current.GoToAsync(".."); // go back one step
    // }


    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
