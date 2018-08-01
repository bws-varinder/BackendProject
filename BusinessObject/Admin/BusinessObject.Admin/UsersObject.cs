using System.Collections.Generic;

namespace BusinessObject.Admin
{
    public class ResultObject
    {
        public int Status { get; set; }
        public string Message { get; set; }
    }

    public class EventObject
    {
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string CategoryName { get; set; }
        public string EventTime { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Hour { get; set; }
        public string Minute { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string EventDescription { get; set; }
        public string YoutubeUrl { get; set; }
    }

    public class UserListReturn
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public List<EventObject> details { get; set; }
    }
}
