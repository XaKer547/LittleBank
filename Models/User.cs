using LittleBank.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBank.Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }

        [InverseProperty(nameof(Models.Client.User))]
        public Client Client { get; set; }

        [InverseProperty(nameof(Models.Employee.User))]
        public Employee Employee { get; set; }
    }
}