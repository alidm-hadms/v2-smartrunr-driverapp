using Microsoft.Maui.Controls;

namespace DriverApp.Views
{
    public partial class RazorpayPage : ContentPage
    {
        private readonly string _orderId;
        private readonly double _amount;
        private readonly string _driverMobile;
        private readonly string _razorpayKey;

        public event Action<string, string, string> PaymentSuccess;

        public RazorpayPage(string orderId, double amount, string driverMobile, string razorpayKey)
        {
            InitializeComponent();

            _orderId = orderId;
            _amount = amount;
            _driverMobile = driverMobile;
            _razorpayKey = razorpayKey;

            RazorpayWebView.Source = new HtmlWebViewSource
            {
                Html = File.ReadAllText("Resources/Raw/razorpay_checkout.html")
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            string js = $"openCheckout('{_orderId}', {_amount}, '{_razorpayKey}', '{_driverMobile}')";
            RazorpayWebView.Eval(js);
        }

        private void RazorpayWebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith("razorpay-callback://success"))
            {
                e.Cancel = true;

                var query = System.Web.HttpUtility.ParseQueryString(new Uri(e.Url).Query);
                var paymentId = query["payment_id"];
                var orderId = query["order_id"];
                var signature = query["signature"];

                PaymentSuccess?.Invoke(paymentId, orderId, signature);
                Navigation.PopModalAsync(); // close Razorpay page
            }
        }
    }
}
