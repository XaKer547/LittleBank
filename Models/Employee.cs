using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBank.Api.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimyc { get; set; }

        [NotMapped]
        public string ShortName => $"{Surname} {Name[0]}. {Patronimyc[0]}.";

        public string PhoneNumber { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string Address { get; set; }

        public int? UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }
    }
}