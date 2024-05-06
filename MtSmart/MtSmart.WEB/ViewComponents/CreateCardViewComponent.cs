using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.BusinessModels;
using MtSmart.WEB.Models.ViewModels;

namespace MtSmart.WEB.ViewComponents
{
    public class CreateCardViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int employeeId)
        {
            var model = GetModel(employeeId);

            return View("CreateCard", model);
        }

        private AddCard GetModel(int employeeId)
        {
            var Date = TermManager.GetDate();
            var min = TermManager.GetMin();

            return new AddCard
            {
                Min = min,
                CardTerm = Date,
                UserId = employeeId
            };
        }
    }
}
