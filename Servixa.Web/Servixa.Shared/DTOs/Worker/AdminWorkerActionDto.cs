using Servixa.Shared.Enums;

namespace Servixa.Shared.DTOs.Worker
{
    public class AdminWorkerActionDto
    {
        public int WorkerId { get; set; }
        public WorkerStatus Action { get; set; }
    }
}
