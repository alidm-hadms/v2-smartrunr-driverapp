namespace DriverApp
{
    //Like sessions to maintain the mobile number and JWT token
    public static class AppState
    {
        public static string? currentDriverMobileNumber { get; set; }
        public static string? currentJwtToken { get; set; }
    }
}