using RepairRequestsService.Repository.Models;

namespace RepairRequestsService.DTO
{
    public class RequestDTO: Request
    {
        public string FailureName { get; set; }
        public string Fio { get; set; }
    }
}
