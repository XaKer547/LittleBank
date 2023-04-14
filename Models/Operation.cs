using LittleBank.Api.Models.Enums;

namespace LittleBank.Api.Models
{
    public class Operation
    {
        public int Id { get; set; }
        public OperationTypes Type { get; set; }
        public double Value { get; set; }
    }
}
