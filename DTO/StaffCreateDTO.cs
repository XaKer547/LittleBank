using System.ComponentModel.DataAnnotations.Schema;

namespace LittleBank.Api.DTO
{
    public class StaffCreateDTO
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronimyc { get; set; }
        public string PhoneNumber { get; set; }
        public string PassportSeries { get; set; }
        public string PassportNumber { get; set; }
        public string Address { get; set; }
    }
}
