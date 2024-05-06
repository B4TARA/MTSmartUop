using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.Interfaces;
using MtSmart.WEB.Models.ViewModels;
using StatusCodes = MtSmart.BLL.Enums.StatusCodes;

namespace MtSmart.WEB.ViewComponents
{
    public class ListSubordinateEmployeesViewComponent : ViewComponent
    {
        private readonly ISupervisorService _supervisorService;

        public ListSubordinateEmployeesViewComponent(ISupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int supervisorId, string supervisorName)
        {
            try
            {
                var model = await GetModelAsync(supervisorId, supervisorName);

                return View("ListSubordinateEmployees", model);
            }
            catch (Exception ex)
            {
                //обработать ошибку
                return View("Error", "Home");
            }
        }

        private async Task<ListSubordinateEmployees> GetModelAsync(int supervisorId, string supervisorName)
        {
            var model = new ListSubordinateEmployees();

            // Получаем список подчиненных сотрудников
            var getSubordinateEmployeesResponse = await _supervisorService.GetSubordinateEmployees(supervisorId);

            if (getSubordinateEmployeesResponse.StatusCode != StatusCodes.OK)
            {
                // Если статус код не равен OK, выбрасываем исключение с описанием ошибки
                throw new Exception($"Error while getting subordinate employees: {getSubordinateEmployeesResponse.Description}");
            }

            var employees = getSubordinateEmployeesResponse.Data;

            // Проходим по корневым руководителям (те, у которых SupervisorName == supervisorName)
            foreach (var rootEmployee in employees.Where(e => e.SupervisorName == supervisorName))
            {
                var rootViewModelObject = new ListSubordinateEmployees.RootEmployee
                {
                    Id = rootEmployee.Id,
                    Name = rootEmployee.Name,
                };

                // Добавляем подчиненных сотрудников
                foreach(var nestedEmployee in employees.Where(e => e.SupervisorName == rootEmployee.Name))
                {
                    var nestedViewModelObject = new ListSubordinateEmployees.NestedEmployee
                    {
                        Id = nestedEmployee.Id,
                        Name = nestedEmployee.Name,
                    };

                    rootViewModelObject.nestedEmployees.Add(nestedViewModelObject);
                }

                // Добавляем корневого руководителя в модель
                model.rootEmployees.Add(rootViewModelObject);
            }

            return model;
        }
    }
}
