using Newtonsoft.Json;

public class Booking
{
    public string firstname { get; set; }
    public string lastname { get; set; }
    public int totalprice { get; set; }
    public bool depositpaid { get; set; }
    public BookingDates bookingdates { get; set; }
    public string additionalneeds { get; set; }
}


public class BookingDates
{
    public string checkin { get; set; }
    public string checkout { get; set; }
}

public class BookingAndID
{
    public int bookingid { get; set; }
    public Booking booking { get; set; }
}

public class BookingIdResponse
{
    public int bookingid { get; set; }
}