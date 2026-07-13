namespace Servixa.Shared.DTOs.Task
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal AvgCost { get; set; }
        public int AvgTime { get; set; }
        public int SpecialtyId { get; set; }
    }
}
