namespace LittleBank.Api.Models
{
    public class Credit
    {
        public int Id { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime EndDate { get; set; }

        public double Sum { get; set; }

        public Client Client { get; set; }
    }
}
