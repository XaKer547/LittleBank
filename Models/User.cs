using LittleBank.Api.Models.Enums;

namespace LittleBank.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}