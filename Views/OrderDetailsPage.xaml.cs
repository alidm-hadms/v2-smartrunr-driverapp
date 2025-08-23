using Microsoft.Maui.Controls;

namespace DriverApp.Views
{
    [QueryProperty(nameof(BookingRefNumber), "bookingRefNumber")]
    public partial class OrderDetailsPage : ContentPage
    {
        private string _bookingRefNumber;
        public string BookingRefNumber
        {
            get => _bookingRefNumber;
            set
            {
                if (_bookingRefNumber == value)
                    return;

                _bookingRefNumber = value;

                // 🔹 Set BindingContext when property is received from Shell
                BindingContext = new OrderDetailsViewModel(_bookingRefNumber);
            }
        }

        // 🔹 Required for Shell Navigation
        public OrderDetailsPage()
        {
            InitializeComponent();
        }

        // 🔹 Optional for manual navigation (if you ever use PushAsync)
        public OrderDetailsPage(string bookingRefNumber) : this()
        {
            BookingRefNumber = bookingRefNumber; 
        }
    }
}
