using MtSmart.BLL.DTO;
using MtSmart.BLL.DTO.SupervisorDTOs;
using MtSmart.BLL.Enums;
using MtSmart.BLL.Interfaces;
using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;

namespace MtSmart.BLL.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly IUnitOfWork _unitOfWork;

        public SupervisorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResponse<IEnumerable<User>>> GetSubordinateEmployees(int supervisorId)
        {
            try
            {
                // Получаем информацию о руководителе по его идентификатору
                var supervisor = await _unitOfWork.Users.GetAsync(x => x.Id == supervisorId);

                IEnumerable<User> employees = new List<User>();

                // Если руководитель с полным доступом
                if (supervisor.Role == DAL.Enums.Roles.FullAccessSupervisor)
                {
                    // Получаем список всех сотрудников
                    employees = await _unitOfWork.Users.GetAllAsync(x => x.Id != supervisorId);
                }
                else
                {
                    // Получаем список всех сотрудников, назначенных этому руководителю
                    employees = await _unitOfWork.Users.GetAllAsync(x => x.SupervisorName == supervisor.Name && !x.IsBlocked);
                }

                return new BaseResponse<IEnumerable<User>>()
                {
                    Data = employees,
                    StatusCode = StatusCodes.OK,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<User>>()
                {
                    Description = $"[GetSubordinateEmployees]: {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<int>> GetFirstSubordinateEmployee(int supervisorId)
        {
            try
            {
                // Получаем список подчиненных сотрудников
                var getSubordinateEmployeesResponse = await GetSubordinateEmployees(supervisorId);

                if (getSubordinateEmployeesResponse.StatusCode != StatusCodes.OK)
                {
                    return new BaseResponse<int>()
                    {
                        Description = $"[GetFirstSubordinateEmployee]: {getSubordinateEmployeesResponse.Description}",
                        StatusCode = StatusCodes.InternalServerError
                    };
                }

                var employees = getSubordinateEmployeesResponse.Data;

                // Проверяем, что список сотрудников не пуст
                if (employees.Any())
                {
                    // Возвращаем идентификатор первого подчиненного сотрудника
                    return new BaseResponse<int>()
                    {
                        Data = employees.First().Id,
                        StatusCode = StatusCodes.OK,
                    };
                }
                else
                {
                    return new BaseResponse<int>()
                    {
                        Description = "Подчиненные сотрудники не найдены",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<int>()
                {
                    Description = $"[GetFirstSubordinateEmployee]: {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<bool>> CheckAccessToEmployee(int userId, int employeeId)
        {
            try
            {              
                // Руководитель хочет получить карточки сотрудника
                // Получаем информацию о руководителе по его идентификатору
                var supervisor = await _unitOfWork.Users.GetAsync(x => x.Id == userId);

                IEnumerable<User> employees = new List<User>();

                // Если руководитель с полным доступом
                if (supervisor.Role == DAL.Enums.Roles.FullAccessSupervisor)
                {
                    // Получаем список всех сотрудников
                    employees = await _unitOfWork.Users.GetAllAsync();
                }
                else
                {
                    // Получаем список всех сотрудников, назначенных этому руководителю
                    employees = await _unitOfWork.Users.GetAllAsync(x => x.SupervisorName == supervisor.Name && !x.IsBlocked);
                }

                // Проверяем, что список сотрудников не является null
                if (employees != null)
                {
                    // Проверяем, содержит ли список указанного сотрудника
                    bool hasAccess = employees.Any(x => x.Id == employeeId);

                    return new BaseResponse<bool>()
                    {
                        Data = hasAccess,
                        StatusCode = StatusCodes.OK
                    };
                }
                else
                {
                    return new BaseResponse<bool>()
                    {
                        Data = false,
                        Description = "Список сотрудников пуст",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Data = false,
                    Description = $"[CheckAccessToEmployee]: {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<List<GetReportDTO>>> GetReportView(int supervisorId, string viewDateInterval)
        {
            try
            {
                var getReportDTOs = new List<GetReportDTO>();

                var date1 = new DateOnly();
                var date2 = new DateOnly();

                if (viewDateInterval != null && viewDateInterval!.Contains("-"))
                {
                    if (DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[0])) < DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[1])))
                    {
                        date1 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[0]));
                        date2 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[1]));
                    }
                    else if (DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[0])) > DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[1])))
                    {
                        date1 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[1]));
                        date2 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[0]));
                    }
                    else
                    {
                        date1 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[0]));
                        date2 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval.Split(" - ")[1]));
                    }
                }
                else if (viewDateInterval != null)
                {
                    date1 = DateOnly.FromDateTime(Convert.ToDateTime(viewDateInterval));
                    date2 = date1;
                }

                date2 = new DateOnly(date2.Year, date2.Month, DateTime.DaysInMonth(date2.Year, date2.Month));

                // Получаем список подчиненных сотрудников
                var getSubordinateEmployeesResponse = await GetSubordinateEmployees(supervisorId);

                if (getSubordinateEmployeesResponse.StatusCode != StatusCodes.OK)
                {
                    return new BaseResponse<List<GetReportDTO>>()
                    {
                        Description = $"[GetReportView]: {getSubordinateEmployeesResponse.Description}",
                        StatusCode = StatusCodes.InternalServerError
                    };
                }

                var employees = getSubordinateEmployeesResponse.Data;

                foreach(var employee in employees)
                {
                    var reportCards = await _unitOfWork.Cards.GetAllAsync(x => x.UserId == employee.Id && x.ReadyToReport && x.FactTerm <= date2 && x.FactTerm >= date1);

                    foreach(var reportCard in reportCards)
                    {
                        var employeeQualityAssessment = await _unitOfWork.AssessmentQualityResults.GetAsync(x => x.Id == reportCard.EmployeeQualityAssessment);
                        var employeeTermAssessment = await _unitOfWork.AssessmentQualityResults.GetAsync(x => x.Id == reportCard.EmployeeTermAssessment);
                        var supervisorQualityAssessment = await _unitOfWork.AssessmentQualityResults.GetAsync(x => x.Id == reportCard.SupervisorQualityAssessment);
                        var supervisorTermAssessment = await _unitOfWork.AssessmentQualityResults.GetAsync(x => x.Id == reportCard.SupervisorTermAssessment);

                        var getReportDTO = new GetReportDTO
                        {
                            EmployeeSspName = employee.SspName,
                            EmployeeName = employee.Name,
                            EmployeePosition = employee.Position,
                            SupervisorName = employee.SupervisorName,

                            CardName = reportCard.Name,
                            CardRequirement = reportCard.Requirement,
                            CardStartTerm = reportCard.StartTerm.ToString(),
                            CardFactTerm = reportCard.FactTerm.ToString(),
                            
                            EmployeeQualityAssessmentText = employeeQualityAssessment.Text,
                            EmployeeTermAssessmentText = employeeTermAssessment.Text,
                            HoursOfWork = (int)reportCard.HoursOfWork,
                            EmployeeComment = reportCard.EmployeeComment,

                            SupervisorQualityAssessmentText = supervisorQualityAssessment.Text,
                            SupervisorTermAssessmentText = supervisorTermAssessment.Text,
                            SupervisorComment = reportCard.SupervisorComment,

                            SupervisorQualityAssessmentValue  = supervisorQualityAssessment.Value,
                            SupervisorTermAssessmentValue = supervisorTermAssessment.Value,
                        };

                        getReportDTOs.Add(getReportDTO);
                    }
                }             

                return new BaseResponse<List<GetReportDTO>>()
                {
                    Data = getReportDTOs,
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<List<GetReportDTO>>()
                {
                    Description = $"[GetReportView] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError,
                };
            }
        }
    }
}
