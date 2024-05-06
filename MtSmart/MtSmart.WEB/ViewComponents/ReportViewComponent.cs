using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.DTO.SupervisorDTOs;
using MtSmart.WEB.Models.ViewModels;

namespace MtSmart.WEB.ViewComponents
{
    public class ReportViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(List<GetReportDTO> getReportDTOs, string viewDateInterval)
        {
            ViewBag.ViewDate = viewDateInterval;

            var getReportViewModelList = new List<GetReportViewModel>();

            foreach (var getReportDTO in getReportDTOs)
            {
                var getReportViewModel = new GetReportViewModel
                {
                    EmployeeSspName = getReportDTO.EmployeeSspName,
                    EmployeeName = getReportDTO.EmployeeName,
                    EmployeePosition = getReportDTO.EmployeePosition,
                    SupervisorName = getReportDTO.SupervisorName,

                    CardName = getReportDTO.CardName,
                    CardRequirement = getReportDTO.CardRequirement,
                    CardStartTerm = getReportDTO.CardStartTerm,
                    CardFactTerm = getReportDTO.CardFactTerm,

                    EmployeeQualityAssessmentText = getReportDTO.EmployeeQualityAssessmentText,
                    EmployeeTermAssessmentText = getReportDTO.EmployeeTermAssessmentText,
                    HoursOfWork = getReportDTO.HoursOfWork,
                    EmployeeComment = getReportDTO.EmployeeComment,

                    SupervisorQualityAssessmentText = getReportDTO.SupervisorQualityAssessmentText,
                    SupervisorTermAssessmentText = getReportDTO.SupervisorTermAssessmentText,
                    SupervisorComment = getReportDTO.SupervisorComment,

                    SupervisorQualityAssessmentValue = getReportDTO.SupervisorQualityAssessmentValue,
                    SupervisorTermAssessmentValue = getReportDTO.SupervisorTermAssessmentValue,
                };

                getReportViewModelList.Add(getReportViewModel);
            }            

            return View("MonthReport", getReportViewModelList);
        }
    }
}
