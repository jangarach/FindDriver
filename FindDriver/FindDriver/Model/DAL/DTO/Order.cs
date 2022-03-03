using System.ComponentModel.DataAnnotations.Schema;

namespace FindDriver.Api.Model.DAL.DTO
{
    public class Order
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("from_city_id")]
        public int FromCityId { get; set; }
        [Column("to_city_id")]
        public int ToCityId { get; set; }
        [Column("datestamp")]
        public DateTime Datestamp { get; }
        [Column("dateout")]
        public DateTime Dateout { get; set; }
        [Column("comment")]
        public string? Comment { get; set; }
        [Column("order_type_id")]
        public int OrderTypeId { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
    }
}
