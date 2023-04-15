using Azure;
using LittleBank.Api.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBank.Api.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public double Sum { get; set; }
        public CardTypes Type { get; set; }
        public bool IsActive { get; set; } = true;

        public Client Client { get; set; }
        
        public virtual ICollection<Operation> Operations { get; set; } = new HashSet<Operation>();
    }
}