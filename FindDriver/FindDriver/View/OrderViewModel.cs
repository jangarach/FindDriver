namespace FindDriver.Api.View
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
    }
}
