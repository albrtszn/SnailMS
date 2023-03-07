namespace SnailMS.Models
{
    public class CallDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Number { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string ManagerId { get; set; }
    }
}
