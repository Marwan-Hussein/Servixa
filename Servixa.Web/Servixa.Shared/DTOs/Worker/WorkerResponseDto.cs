namespace Servixa.Shared.DTOs.Worker
{
    public class WorkerResponseDto
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int SpecialtyId { get; set; }
        public double AverageRating { get; set; }
        public bool IsAvailable { get; set; }
    }
}
