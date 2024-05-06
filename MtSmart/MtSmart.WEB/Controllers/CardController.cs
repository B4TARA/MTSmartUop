using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MtSmart.BLL.DTO.CardDTOs;
using MtSmart.BLL.Interfaces;
using MtSmart.WEB.Models.RequestModels;
using StatusCodes = MtSmart.BLL.Enums.StatusCodes;

namespace MtSmart.WEB.Controllers
{
    public class CardController : Controller
    {
        private readonly ICardService _cardService;
        private readonly ISupervisorService _supervisorService;

        public CardController(ICardService cardService, ISupervisorService supervisorService)
        {
            _cardService = cardService;
            _supervisorService = supervisorService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreateCard(int employeeId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            return ViewComponent("CreateCard", employeeId);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateCard(CreateCardRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != requestModel.UserId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.CreateCard(new CreateCardDTO
            {
                CardName = requestModel.CardName,
                CardRequirement = requestModel.CardRequirement,
                CardTerm = DateOnly.Parse(requestModel.CardTerm),
                UserId = requestModel.UserId,
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Карточка создана!");
            }

            return BadRequest(response.Description);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCardDetails(int employeeId, int cardId)
        {

            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            return ViewComponent("CardDetails", new { cardId, currentUserId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCardAssessment(int employeeId, int cardId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            return ViewComponent("CardAssessment", new { cardId, currentUserId });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetCardHistory(int employeeId, int cardId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != employeeId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, employeeId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            return ViewComponent("CardHistory", new { cardId, currentUserId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateCard(UpdateCardRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != requestModel.UserId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.UpdateCard(new UpdateCardDTO
            {
                CardId = requestModel.CardId,
                CardName = requestModel.CardName,
                CardRequirement = requestModel.CardRequirement,
                CardTerm = DateOnly.Parse(requestModel.CardTerm),
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Карточка изменена!");
            }

            return BadRequest(response.Description);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MoveCard(MoveCardRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != requestModel.UserId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.MoveCard(new MoveCardDTO
            {
                CardId = requestModel.CardId,
                CardName = requestModel.CardName,
                CardRequirement = requestModel.CardRequirement,
                CardTerm = DateOnly.Parse(requestModel.CardTerm),
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Карточка переведена!");
            }

            return BadRequest(response.Description);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RejectCard(RejectCardRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Владелец карточки не может ее отклонить
            // Проверка доступа создания карточки сотруднику
            var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

            // Если пользователь не имеет доступа создавать карточки сотруднику
            if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (checkAccessToEmployeeResponse.Data == false)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var response = await _cardService.RejectCard(new RejectCardDTO
            {
                CardId = requestModel.CardId,
                CardName = requestModel.CardName,
                CardRequirement = requestModel.CardRequirement,
                CardTerm = DateOnly.Parse(requestModel.CardTerm),
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Карточка отклонена!");
            }

            return BadRequest(response.Description);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadFile(UploadFileRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != requestModel.UserId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.UploadFiles(new UploadFileDTO
            {
                FilesToUpload = requestModel.FilesToUpload,
                CardId = requestModel.CardId,
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Файл успешно добавлен!");
            }

            return BadRequest(response.Description);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> DeleteFile(int cardId, int fileId, int userId)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != userId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, userId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.DeleteFile(new DeleteFileDTO
            {
                FileId = fileId,
                CardId = cardId,
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Файл успешно удалён!");
            }

            return BadRequest(response.Description);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddComment(AddCommentRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != requestModel.UserId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.AddComment(new AddCommentDTO
            {
                Comment = requestModel.Comment,
                CardId = requestModel.CardId,
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Комментарий успешно добавлен!");
            }

            return BadRequest(response.Description);
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetSupervisorAssessment(SetSupervisorAssessmentRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Владелец карточки не может выставить оценочное суждение руководителя
            // Проверка доступа создания карточки сотруднику
            var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

            // Если пользователь не имеет доступа создавать карточки сотруднику
            if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
            {
                return RedirectToAction("Error", "Home");
            }
            else if (checkAccessToEmployeeResponse.Data == false)
            {
                return RedirectToAction("AccessDenied", "Home");
            }

            var response = await _cardService.SetSupervisorAssessment(new SetSupervisorAssessmentDTO
            {
                CardId = requestModel.CardId,
                FactTerm = DateOnly.Parse(requestModel.FactTerm),
                SupervisorQualityAssessment = requestModel.SupervisorQualityAssessment,
                SupervisorTermAssessment = requestModel.SupervisorTermAssessment,
                SupervisorComment = requestModel.SupervisorComment,
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Рейтинг успешно выставлен!");
            }

            return BadRequest(response.Description);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SetEmployeeAssessment(SetEmployeeAssessmentRequestModel requestModel)
        {
            var currentUserId = Convert.ToInt32(User.FindFirst("Id").Value);

            // Если не владелец карточки, то нужна проверка доступа
            if (currentUserId != requestModel.UserId)
            {
                // Проверка доступа создания карточки сотруднику
                var checkAccessToEmployeeResponse = await _supervisorService.CheckAccessToEmployee(currentUserId, requestModel.UserId);

                // Если пользователь не имеет доступа создавать карточки сотруднику
                if (checkAccessToEmployeeResponse.StatusCode != StatusCodes.OK)
                {
                    return RedirectToAction("Error", "Home");
                }
                else if (checkAccessToEmployeeResponse.Data == false)
                {
                    return RedirectToAction("AccessDenied", "Home");
                }
            }

            var response = await _cardService.SetEmployeeAssessment(new SetEmployeeAssessmentDTO
            {
                CardId = requestModel.CardId,
                EmployeeQualityAssessment = requestModel.EmployeeQualityAssessment,
                EmployeeTermAssessment = requestModel.EmployeeTermAssessment,
                HoursOfWork = requestModel.HoursOfWork,
                EmployeeComment = requestModel.EmployeeComment,
                UpdaterName = User.FindFirst("Name").Value,
                UpdaterImagePath = User.FindFirst("ImagePath").Value,
            });

            if (response.StatusCode == StatusCodes.OK)
            {
                return Ok("Рейтинг успешно выставлен!");
            }

            return BadRequest(response.Description);
        }
    }
}
