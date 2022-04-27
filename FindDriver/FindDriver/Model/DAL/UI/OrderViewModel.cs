namespace FindDriver.Api.Model.DAL.UI
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        public int FromCityId { get; set; }
        public string FromCityName { get; set; }
        public int ToCityId { get; set; }
        public string ToCityName { get; set; }
        public DateTime DateStamp { get; set; }
        public DateTime DateOut { get; set; }
        public string? Comment { get; set; }
        public int OrderTypeId { get; set; }   
        public string OrderTypeName { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
        public int PassengersCount { get; set; }
        public bool State { get; set; }
    }
}
