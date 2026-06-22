namespace Servixa.Shared.DTOs.Task
{
    public class UpdateTaskDto
    {
        public string? Name { get; set; }
        public decimal AvgCost { get; set; }
        public int AvgTime { get; set; }
        public int SpecialtyId { get; set; }
    }
}
