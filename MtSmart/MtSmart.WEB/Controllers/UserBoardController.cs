using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.Interfaces;
using MtSmart.WEB.Models.ViewModels;
using StatusCodes = MtSmart.BLL.Enums.StatusCodes;

namespace MtSmart.WEB.Controllers
{
    public class UserBoardController : Controller
    {
        private readonly IUserBoardService _userBoardService;
        private readonly ISupervisorService _supervisorService;

        public UserBoardController(IUserBoardService userBoardService, ISupervisorService supervisorService)
        {
            _userBoardService = userBoardService;
            _supervisorService = supervisorService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListEmployeeCards(int employeeId)
        {
            var currentUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа пользователя к карточкам
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа к карточкам
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            // Получение карточек
            var listUserCardsResponse = await _userBoardService.ListUserCards(employeeId);

            if (listUserCardsResponse.StatusCode != StatusCodes.OK)
            {
                return RedirectToAction("Error", "Home");
            }

            var listUserCardsDTO = listUserCardsResponse.Data;

            var listUserCardsViewModel = new ListUserCardsViewModel
            {
                UserId = listUserCardsDTO.UserId,
                UserName = listUserCardsDTO.UserName,
                SspName = listUserCardsDTO.SspName,
                UserImagePath = listUserCardsDTO.UserImagePath,
                Columns = listUserCardsDTO.Columns.Select(column => new ListUserCardsViewModel.Column
                {
                    ColumnId = column.ColumnId,
                    ColumnNumber = column.ColumnNumber,
                    ColumnTitle = column.ColumnTitle,
                    Cards = column.Cards.Select(card => new ListUserCardsViewModel.Card
                    {
                        CardId = card.CardId,
                        CardName = card.CardName,
                        CardRequirement = card.CardRequirement,
                        CardTerm = card.CardTerm,
                        FactTerm = card.FactTerm,
                        SupervisorTermAssessment = card.SupervisorTermAssessment,
                        CountOfComments = card.CountOfComments,
                        CountOfFiles = card.CountOfFiles
                    }).ToList()
                }).ToList()
            };

            return View(listUserCardsViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListEmployeeCardsTable(int employeeId)
        {
            var currentUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа пользователя к карточкам
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа к карточкам
                if (checkAccessToEmployeeResponse.StatusCode != BLL.Enums.StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            // Получение карточек сотрудника
            var listUserCardsResponse = await _userBoardService.ListUserCards(employeeId);

            if (listUserCardsResponse.StatusCode != BLL.Enums.StatusCodes.OK)
            {
                return RedirectToAction("Error", "Home");
            }

            var listUserCardsDTO = listUserCardsResponse.Data;

            var listUserCardsViewModel = new ListUserCardsViewModel
            {
                UserId = listUserCardsDTO.UserId,
                UserName = listUserCardsDTO.UserName,
                SspName = listUserCardsDTO.SspName,
                UserImagePath = listUserCardsDTO.UserImagePath,
                Columns = listUserCardsDTO.Columns.Select(column => new ListUserCardsViewModel.Column
                {
                    ColumnId = column.ColumnId,
                    ColumnNumber = column.ColumnNumber,
                    ColumnTitle = column.ColumnTitle,
                    Cards = column.Cards.Select(card => new ListUserCardsViewModel.Card
                    {
                        CardId = card.CardId,
                        CardName = card.CardName,
                        CardRequirement = card.CardRequirement,
                        CardTerm = card.CardTerm,
                        FactTerm = card.FactTerm,
                        SupervisorTermAssessment = card.SupervisorTermAssessment,
                        CountOfComments = card.CountOfComments,
                        CountOfFiles = card.CountOfFiles
                    }).ToList()
                }).ToList()
            };

            return View(listUserCardsViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ListUserArchivedCards(int employeeId)
        {
            var currentUserId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа пользователя к карточкам
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа к карточкам
                if (checkAccessToEmployeeResponse.StatusCode != BLL.Enums.StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            // Получение архивных карточек сотрудника
            var listUserArchivedCards = await _userBoardService.ListUserArchivedCards(employeeId);

            if (listUserArchivedCards.StatusCode != StatusCodes.OK)
            {
                return RedirectToAction("Error", "Home");
            }

            var listUserArchivedCardsDTO = listUserArchivedCards.Data;

            var listUserArchivedCardsViewModel = new ListUserArchivedCardsViewModel
            {
                UserId = listUserArchivedCardsDTO.UserId,
                UserName = listUserArchivedCardsDTO.UserName,
                SspName = listUserArchivedCardsDTO.SspName,
                UserImagePath = listUserArchivedCardsDTO.UserImagePath,
                ArchivedCards = listUserArchivedCardsDTO.ArchivedCards.Select(card => new ListUserArchivedCardsViewModel.Card
                {
                    CardId = card.CardId,
                    CardName = card.CardName,
                    CardRequirement = card.CardRequirement,
                    CardTerm = card.CardTerm,
                    FactTerm = card.FactTerm,
                    HoursOfWork = card.HoursOfWork,
                    EmployeeQualityAssessment = card.EmployeeQualityAssessment,
                    EmployeeTermAssessment = card.EmployeeTermAssessment,
                    SupervisorQualityAssessment = card.SupervisorQualityAssessment,
                    SupervisorTermAssessment = card.SupervisorTermAssessment,
                    CountOfComments = card.CountOfComments,
                    CountOfFiles = card.CountOfFiles
                }).ToList(),
            };

            return View(listUserArchivedCardsViewModel);
        }
    }
}
