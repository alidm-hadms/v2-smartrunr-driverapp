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

    //Model for recharge wallet
    // API response model for recharge options
    public class RechargeResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public List<RechargeDetails> rechargedetails { get; set; }
    }

    public class RechargeDetails
    {
        public decimal rechargeamount { get; set; }
        public string? additionalinformation { get; set; }
    }
    public class RechargeOption
    {
        public decimal RechargeAmount { get; set; }
        public string? AdditionalInformation { get; set; }
        public bool IsSelected { get; set; }   // <-- helps with RadioButton binding
    }

    // Validate Signature Response
    public class CreateOrderResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string order_id { get; set; }
    }

    public class ValidateSignatureRequest
    {
        public string drivermobilenumber { get; set; }
        public string order_id { get; set; }
        public string razorpaypaymentid { get; set; }
        public double amount { get; set; }
        public string razorpaysignature { get; set; }
    }

    public class ValidateSignatureResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string order_id { get; set; }
    }

    public class WalletBalanceResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public decimal balance { get; set; }
    }



}


