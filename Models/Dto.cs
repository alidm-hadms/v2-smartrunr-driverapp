using System;

namespace DriverApp.Models
{
    public class LoginRequest
    {
        //[JsonPropertyName("driverMobileNumber")]
        public string? driverMobileNumber { get; set; }
        public string? tokenNumber { get; set; }
    }

    public class LoginResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public string? jwtToken { get; set; }
    }
    public class OtpVerificationRequest
    {
        public string? driverMobileNumber { get; set; }
        public string? otp { get; set; }
    }
    public class StandardResponse // use this if response is same for apis
    {
        public string? status { get; set; }
        public string? message { get; set; }
    }
}