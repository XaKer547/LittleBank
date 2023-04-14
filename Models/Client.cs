using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBank.Api.Models
{
    public class Client
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

        public User? User { get; set; }
        public virtual ICollection<Card> Cards { get; set; } = new HashSet<Card>();

        public virtual ICollection<Credit> Credits { get; set; } = new HashSet<Credit>();
    }
}