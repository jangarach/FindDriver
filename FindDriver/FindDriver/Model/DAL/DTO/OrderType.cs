using System.ComponentModel.DataAnnotations.Schema;

namespace FindDriver.Api.Model.DAL.DTO
{
    public class OrderType
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("type_name")]
        public string TypeName { get; set; }
    }
}
