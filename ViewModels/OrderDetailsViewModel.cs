using System;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DriverApp;
using DriverApp.Models;

public class OrderDetailsViewModel : INotifyPropertyChanged
{
    private readonly HttpClient _httpClient;

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



    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string name) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
