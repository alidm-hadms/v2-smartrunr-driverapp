namespace DriverApp
{
    public static class ApiConstant
    {
        // public const string BaseUrl = "https://app.hadmservices.com/api/";
        // public const string LoginEndpoint = "DriverApp/driver-login";
        // public const string OtpVerificationEndpoint = "DriverApp/validate-otp";

        // Add more API endpoints as needed
        public static class Auth
        {
            public const string Login = "DriverApp/driver-login";
            public const string OtpVerification = "DriverApp/validate-otp";

            public const string DayOrderHistory = "DriverApp/day-order-history";
            public const string WeeklyOrderHistory = "DriverApp/weekly-order-history";
            public const string MonthlyOrderHistory = "DriverApp/monthly-order-history";
            public const string OrderSummaryDetails = "DriverApp/order-summary";


        }
        public static class Wallet
        {
            public const string AvailableWalletBalance = "DriverApp/display-wallet-balance";
            public const string TopUpTransactionHistory = "DriverApp/top-up-details";
            public const string AddMoneyToWallet = "DriverApp/add-money-to-wallet";
            public const string TopUpOptions = "DriverApp/add-balance-denominations";

            public const string CreateRazorPayOrderId = "DriverApp/Create-Order-Id-For-Wallet-Recharge";

            public const string ValidateRazorPaySignature = "DriverApp/Validate-Signature-For-Wallet-Recharge";
        }
    }

        
}