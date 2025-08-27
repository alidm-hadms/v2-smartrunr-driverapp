namespace DriverApp.Models
{
    // Base Order Model
    public class OrderItem
    {
        public string? bookingrefnumber { get; set; }
        public string? date { get; set; }
        public string? time { get; set; }
        public decimal amount { get; set; }
        public string? pickuppoint { get; set; }
        public string? destination { get; set; }
        public string? status { get; set; }
        public bool ismiddledrops { get; set; }

    }

    // Day Response
    public class DayOrdersResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public int completedorders { get; set; }
        public decimal totalearnings { get; set; }
        public string? date { get; set; }
        public List<OrderItem> dayOrdersList { get; set; }
    }

    // Week Response
    public class WeeklyOrdersResponse
    {
        public string? status { get; set; }
        public string? message { get; set; }
        public int completedorders { get; set; }
        public decimal totalearnings { get; set; }
        public List<OrderItem> weeklyOrdersList { get; set; }
    }

    // Month Response
    public class MonthlyOrdersResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public int completedorders { get; set; }
        public decimal totalearnings { get; set; }
        public List<OrderItem> monthlyOrdersList { get; set; }
    }
    
    public class OrderDetailsResponse
    {
        public string? rideStatus { get; set; }
        public string? status { get; set; }
        public string? message { get; set; }
        public string? bookingRefNumber { get; set; }
        public string? rideDate { get; set; }
        public string? pickUpAddress { get; set; }
        public string? dropAddress { get; set; }
        public string? pickUpTime { get; set; }
        public string? dropTime { get; set; }
        public double distance { get; set; }
        public decimal fareAmount { get; set; }
        public List<MiddleStop> middleStops { get; set; }
    }

    public class MiddleStop
    {
        public string? middleStop { get; set; }
    }

}
