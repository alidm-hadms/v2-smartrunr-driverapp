namespace DriverApp.Models
{
    public class Wallet
    {

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


