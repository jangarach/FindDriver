using System.ComponentModel.DataAnnotations.Schema;

namespace FindDriver.Api.Model.DAL.DTO
{
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("fullname")]
        public string Fullname { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("phone_number")]
        public string PhoneNumber { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}
