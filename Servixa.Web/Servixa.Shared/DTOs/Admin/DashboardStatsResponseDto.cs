namespace Servixa.Shared.DTOs.Admin
{
    public class DashboardStatsResponseDto
    {
        public int TotalClients { get; set; }
        public int TotalWorkers { get; set; }
        public int PendingWorkers { get; set; }
        public int TotalBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalUsers { get; set; }
    }
}
