using MtSmart.BLL.DTO.SupervisorDTOs;
using MtSmart.DAL.Entities;

namespace MtSmart.BLL.Interfaces
{
    public interface ISupervisorService
    {
        Task<IBaseResponse<IEnumerable<User>>> GetSubordinateEmployees(int supervisorId);

        Task<IBaseResponse<int>> GetFirstSubordinateEmployee(int supervisorId);

        Task<IBaseResponse<bool>> CheckAccessToEmployee(int supervisorId, int employeeId);

        Task<IBaseResponse<List<GetReportDTO>>> GetReportView(int supervisorId, string viewDateInterval);
    }
}
