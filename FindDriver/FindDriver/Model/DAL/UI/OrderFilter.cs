namespace FindDriver.Api.Model.DAL.UI
{
    public class OrderFilter
    {
        public Guid? UserId { get; set; }
        public int? FromCityId { get;set; }
        public int? ToCityId { get;set;}
        public int? OrderTypeId { get;set; }
        public DateTime DateOut { get;set; }
        public bool? State { get; set; }

    }
}
