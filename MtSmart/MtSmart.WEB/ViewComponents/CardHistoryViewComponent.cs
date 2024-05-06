using Microsoft.AspNetCore.Mvc;
using MtSmart.DAL.Interfaces;
using MtSmart.WEB.Models.ViewModels;

namespace MtSmart.WEB.ViewComponents
{
    public class CardHistoryViewComponent : ViewComponent   
    {
        private readonly IUnitOfWork _unitOfWork;

        public CardHistoryViewComponent(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IViewComponentResult> InvokeAsync(int cardId, int currentUserId)
        {
            var model = await GetModelAsync(cardId, currentUserId);

            return View("CardHistory", model);
        }

        private async Task<CardDetails> GetModelAsync(int cardId, int currentUserId)
        {
            var user = await _unitOfWork.Users.GetAsync(x => x.Id == currentUserId);
            var card = await _unitOfWork.Cards.GetAsync(x => x.Id == cardId, includeProperties: new string[] { "Comments", "Files", "Updates" });
            var column = await _unitOfWork.Columns.GetAsync(x => x.Id == card.ColumnId);

            var assessmentQualityResults = await _unitOfWork.AssessmentQualityResults.GetAllAsync();
            var assessmentTermResults = await _unitOfWork.AssessmentTermResults.GetAllAsync();

            var cardDetails = new CardDetails
            {
                CardId = card.Id,
                CardName = card.Name,
                CardRequirement = card.Requirement,
                CardTerm = card.Term,
                FactTerm = card.FactTerm,
                HoursOfWork = card.HoursOfWork,
                EmployeeQualityAssessment = card.EmployeeQualityAssessment,
                EmployeeTermAssessment = card.EmployeeTermAssessment,
                SupervisorQualityAssessment = card.SupervisorQualityAssessment,
                SupervisorTermAssessment = card.SupervisorTermAssessment,
                EmployeeComment = card.EmployeeComment,
                SupervisorComment = card.SupervisorComment,
                Column = column!.Number,
                ColumnId = column.Id,
                UserId = card.UserId,
                IsRelevant = card.IsRelevant,
                IsDeleted = card.IsDeleted,
                Comments = card.Comments,
                Files = card.Files,
                Updates = card.Updates,

                CurrentUserId = currentUserId,
                IsFullAccessSupervisor = user.Role == DAL.Enums.Roles.FullAccessSupervisor ? true : false,
            };

            // Проверка на необходимость предоставления доступа к оценке сотрудником
            // Если сотрудник не выставлял оценочное суждение
            if ((card.UserId == currentUserId || user.Role == DAL.Enums.Roles.FullAccessSupervisor) && (card.EmployeeQualityAssessment == null || card.EmployeeTermAssessment == null || card.HoursOfWork == null) && column.Number == 4)
            {
                cardDetails.IsNeedEmployeeAssessment = true;
            }
            //// Если рководитель выставлял оценочное суждение о переносе карточки
            //else if((card.UserId == currentUserId || user.Role == DAL.Enums.Roles.FullAccessSupervisor) && ((card.SupervisorTermAssessment == 2 || card.SupervisorTermAssessment == 3) && column.Number == 4))
            //{
            //    cardDetails.IsNeedEmployeeAssessment = true;
            //}

            // Проверка на необходимость предоставления доступа к оценке руководителем
            // Если руководитель не выставлял оценочное суждение
            else if ((card.UserId != currentUserId || user.Role == DAL.Enums.Roles.FullAccessSupervisor) && (card.SupervisorQualityAssessment == null || card.SupervisorTermAssessment == null) && column.Number == 5)
            {
                cardDetails.IsNeedSupervisorAssessment = true;
            }
            //// Если руководитель выставлял оценочное суждение о переносе карточки
            //else if((card.UserId != currentUserId || user.Role == DAL.Enums.Roles.FullAccessSupervisor) && ((card.SupervisorTermAssessment == 2 || card.SupervisorTermAssessment == 3) && column.Number == 5))
            //{
            //    cardDetails.IsNeedSupervisorAssessment = true;
            //}

            return cardDetails;
        }
    }
}
