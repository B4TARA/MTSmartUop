using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.Interfaces;
using StatusCodes = MtSmart.BLL.Enums.StatusCodes;

namespace MtSmart.WEB.Controllers
{
    public class SupervisorController : Controller
    {
        private readonly ISupervisorService _supervisorService;

        public SupervisorController(ISupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor, Combined, FullAccessSupervisor")]
        public async Task<IActionResult> GetReportViewComponent(string viewDateInterval)
        {
            try
            {
                var currentUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

                var getReportViewResponse = await _supervisorService.GetReportView(currentUserId, viewDateInterval);

                if (getReportViewResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }

                return ViewComponent("Report", new { getReportDTOs = getReportViewResponse.Data, viewDateInterval = viewDateInterval });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Supervisor, Combined, FullAccessSupervisor")]
        public IActionResult GetReportView()
        {
            return View();
        }

        //[HttpPost]
        //[Authorize(Roles = "Urp, Umst, Uprb, Uko, SupervisorUdpo, SupervisorUprb, SupervisorUko")]
        //public async Task<JsonResult> SaveReport(string viewDateInterval)
        //{
        //    try
        //    {
        //        int serviceNumber = Convert.ToInt32(User.FindFirst("service_number").Value);


        //        var structureType = User.FindFirst("structure_type").Value;


        //        var response = await _reportService.SaveReport3part2(viewDate, serviceNumber, structureType);


        //        Response.StatusCode = (int)response.StatusCode;


        //        if (response.StatusCode != Domain.Enums.StatusCodes.OK)
        //        {
        //            return Json("Упс... Что-то пошло не так: " + response.Description);
        //        }


        //        return Json(response.Data);
        //    }


        //    catch (Exception ex)
        //    {
        //        return Json("Упс... Что-то пошло не так : " + ex.Message);
        //    }
        //}
    }
}
