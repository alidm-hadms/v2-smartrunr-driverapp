using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using DriverApp.Models;

namespace DriverApp.ViewModels
{
    public class DateRangeItem : INotifyPropertyChanged
    {
        private bool _isSelected;

        public string? Label { get; set; }
        public DateTime Date { get; set; } // For Day
        public DateTime StartDate { get; set; } // For Week
        public DateTime EndDate { get; set; }   // For Week
        public int Year { get; set; }           // For Month
        public int Month { get; set; }          // For Month
        public string? orderHistoryType { get; set; } // Completed, Cancelled, All Orders

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            }
        }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    

    public class OrderItem
    {
        public string? BookingRefNumber { get; set; }
        public string? Date { get; set; }
        public string? Time { get; set; }
        public decimal Amount { get; set; }
        public string? PickupPoint { get; set; }
        public string? Destination { get; set; }
        public string? Status { get; set; }
        public bool IsMiddleDrops { get; set; }
    }

    public class EarningsViewModel : INotifyPropertyChanged
    {
        private DateRangeItem _selectedRange;
        private ObservableCollection<OrderItem> _orders;

        public ObservableCollection<DateRangeItem> DateRanges { get; set; }
        public ObservableCollection<OrderItem> Orders
        {
            get => _orders;
            set { _orders = value; OnPropertyChanged(); }
        }

        public DateRangeItem SelectedRange
        {
            get => _selectedRange;
            set
            {
                if (_selectedRange != value)
                {
                    if (_selectedRange != null)
                        _selectedRange.IsSelected = false;

                    _selectedRange = value;

                    if (_selectedRange != null)
                        _selectedRange.IsSelected = true;

                    OnPropertyChanged();
                    _ = FetchOrdersAsync(); // Call API when selection changes
                }
            }
        }

        private int _completedOrders;
        public int CompletedOrders
        {
            get => _completedOrders;
            set => SetProperty(ref _completedOrders, value);
        }

        private decimal _totalEarnings;
        public decimal TotalEarnings
        {
            get => _totalEarnings;
            set => SetProperty(ref _totalEarnings, value);
        }

        private string _message;
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }


        public EarningsViewModel()
        {
            LoadDayRanges(); // default
        }

        // Load Days
        public void LoadDayRanges()
        {
            DateRanges = new ObservableCollection<DateRangeItem>();
            var today = DateTime.Today;

            for (int i = 4; i >= 0; i--)
            {
                var day = today.AddDays(-i);
                DateRanges.Add(new DateRangeItem
                {
                    Label = day.ToString("dd-MMM"),
                    Date = day,
                    IsSelected = i == 0
                });
            }

            SelectedRange = DateRanges[^1];
            OnPropertyChanged(nameof(DateRanges));
        }

        // Load Weeks
        public void LoadWeekRanges()
        {
            DateRanges = new ObservableCollection<DateRangeItem>();
            var today = DateTime.Today;
            var startOfWeek = today.AddDays(-(int)today.DayOfWeek + (int)DayOfWeek.Monday);

            for (int i = 3; i >= 0; i--)
            {
                var start = startOfWeek.AddDays(-7 * i);
                var end = start.AddDays(6);

                DateRanges.Add(new DateRangeItem
                {
                    Label = $"{start:MMM dd} - {end:dd}",
                    StartDate = start,
                    EndDate = end,
                    IsSelected = i == 0
                });
            }

            SelectedRange = DateRanges[^1];
            OnPropertyChanged(nameof(DateRanges));
        }

        // Load Months
        public void LoadMonthRanges()
        {
            DateRanges = new ObservableCollection<DateRangeItem>();
            var today = DateTime.Today;

            for (int i = 5; i >= 0; i--)
            {
                var month = today.AddMonths(-i);

                DateRanges.Add(new DateRangeItem
                {
                    Label = month.ToString("MMM-yyyy"),
                    Year = month.Year,
                    Month = month.Month,
                    IsSelected = i == 0
                });
            }

            SelectedRange = DateRanges[^1];
            OnPropertyChanged(nameof(DateRanges));
        }

        // API Call
        private async Task FetchOrdersAsync()
        {
            if (SelectedRange == null) return;

            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri(ApiConfig.BaseUrl); // ✅ Environment-based BaseUrl

                string jsonBody = "";
                string endpoint = "";

                if (SelectedRange.Date != default) // Day
                {
                    endpoint = ApiConstant.Auth.DayOrderHistory;
                    jsonBody = JsonConvert.SerializeObject(new
                    {
                        driverMobileNumber = "9743375766",
                        date = SelectedRange.Date,
                        orderHistoryType = SelectedRange.orderHistoryType //"Completed"
                    });
                }
                else if (SelectedRange.StartDate != default && SelectedRange.EndDate != default) // Week
                {
                    endpoint = ApiConstant.Auth.WeeklyOrderHistory;
                    jsonBody = JsonConvert.SerializeObject(new
                    {
                        driverMobileNumber = "9743375766",
                        formDate = SelectedRange.StartDate,
                        toDate = SelectedRange.EndDate,
                        orderHistoryType = SelectedRange.orderHistoryType //"Completed"
                    });
                }
                else if (SelectedRange.Year > 0 && SelectedRange.Month > 0) // Month
                {
                    endpoint = ApiConstant.Auth.MonthlyOrderHistory;
                    jsonBody = JsonConvert.SerializeObject(new
                    {
                        driverMobileNumber = "9743375766",
                        year = SelectedRange.Year,
                        monthNumber = SelectedRange.Month,
                        orderHistoryType = SelectedRange.orderHistoryType //"Completed"
                    });
                }

                var response = await client.PostAsync(endpoint,
                    new StringContent(jsonBody, Encoding.UTF8, "application/json"));

                var result = await response.Content.ReadAsStringAsync();

                if (endpoint == ApiConstant.Auth.DayOrderHistory)
                {
                    var dayResponse = JsonConvert.DeserializeObject<DayOrdersResponse>(result);
                    Orders = new ObservableCollection<OrderItem>();
                    if (dayResponse.dayOrdersList != null)
                    {
                        CompletedOrders = dayResponse.completedorders;
                        TotalEarnings = dayResponse.totalearnings;
                        Message = dayResponse.message;
                        foreach (var o in dayResponse.dayOrdersList)
                        {
                            Orders.Add(new OrderItem
                            {
                                BookingRefNumber = o.bookingrefnumber,
                                Date = o.date,
                                Time = o.time,
                                Amount = o.amount,
                                PickupPoint = o.pickuppoint,
                                Destination = o.destination,
                                Status = o.status,
                                IsMiddleDrops = o.ismiddledrops
                            });
                        }
                    }
                }
                else if (endpoint == ApiConstant.Auth.WeeklyOrderHistory)
                {
                    var weekResponse = JsonConvert.DeserializeObject<WeeklyOrdersResponse>(result);
                    Orders = new ObservableCollection<OrderItem>();
                    if (weekResponse.weeklyOrdersList != null)
                    {
                        CompletedOrders = weekResponse.completedorders;
                        TotalEarnings = weekResponse.totalearnings;
                        Message = weekResponse.message;

                        foreach (var o in weekResponse.weeklyOrdersList)
                        {
                            Orders.Add(new OrderItem
                            {
                                BookingRefNumber = o.bookingrefnumber,
                                Date = o.date,
                                Time = o.time,
                                Amount = o.amount,
                                PickupPoint = o.pickuppoint,
                                Destination = o.destination,
                                Status = o.status,
                                IsMiddleDrops = o.ismiddledrops
                            });
                        }
                    }
                }
                else if (endpoint == ApiConstant.Auth.MonthlyOrderHistory)
                {
                    var monthResponse = JsonConvert.DeserializeObject<MonthlyOrdersResponse>(result);
                    Orders = new ObservableCollection<OrderItem>();
                    if (monthResponse.monthlyOrdersList != null)
                    {
                        CompletedOrders = monthResponse.completedorders;
                       TotalEarnings = monthResponse.totalearnings;
                       Message = monthResponse.message;

                        foreach (var o in monthResponse.monthlyOrdersList)
                        {
                            Orders.Add(new OrderItem
                            {
                                BookingRefNumber = o.bookingrefnumber,
                                Date = o.date,
                                Time = o.time,
                                Amount = o.amount,
                                PickupPoint = o.pickuppoint,
                                Destination = o.destination,
                                Status = o.status,
                                IsMiddleDrops = o.ismiddledrops
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("API Error: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    // Response Models
    // public class DayResponse
    // {
    //     public string status { get; set; }
    //     public string message { get; set; }
    //     public int completedorders { get; set; }
    //     public decimal totalearnings { get; set; }
    //     public string date { get; set; }
    //     public OrderItem[] dayOrdersList { get; set; }
    // }

    // public class WeekResponse
    // {
    //     public string status { get; set; }
    //     public string message { get; set; }
    //     public int completedorders { get; set; }
    //     public decimal totalearnings { get; set; }
    //     public OrderItem[] weeklyOrdersList { get; set; }
    // }

    // public class MonthResponse
    // {
    //     public string status { get; set; }
    //     public string message { get; set; }
    //     public int completedorders { get; set; }
    //     public decimal totalearnings { get; set; }
    //     public OrderItem[] monthlyOrdersList { get; set; }
    // }
}
