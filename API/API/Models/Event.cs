namespace API.Models
{
    public class Event
    {
        public string EventID { get; set; }

        public string EventName { get; set; }

        public string LocationID { get; set; }

        public string Time { get; set; }

        public bool Current { get; set; }
    }
}
