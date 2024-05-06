using MtSmart.BLL.DTO;
using MtSmart.BLL.DTO.UserBoardDTOs;
using MtSmart.BLL.Enums;
using MtSmart.BLL.Infrastructure.Configuration;
using MtSmart.BLL.Interfaces;
using MtSmart.DAL.Interfaces;

namespace MtSmart.BLL.Services
{
    public class UserBoardService : IUserBoardService
    {
        private readonly string _defaultImagePath;
        private readonly IUnitOfWork _unitOfWork;

        public UserBoardService(IUnitOfWork unitOfWork, ImageSettings imageSettings)
        {
            _unitOfWork = unitOfWork;
            _defaultImagePath = imageSettings.DefaultImagePath;
        }

        public async Task<IBaseResponse<ListUserCardsDTO>> ListUserCards(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetAsync(x => x.Id == userId, includeProperties: "Columns.Cards");
                if (user is null)
                {
                    return new BaseResponse<ListUserCardsDTO>()
                    {
                        Description = "Cant find user",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var model = new ListUserCardsDTO
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    SspName = user.SspName,
                    UserImagePath = user.ImagePath ?? _defaultImagePath,
                    Columns = user.Columns.Select(column => new ListUserCardsDTO.Column
                    {
                        ColumnId = column.Id,
                        ColumnNumber = column.Number,
                        ColumnTitle = column.Title,
                        Cards = column.Cards
                            .Where(card => card.IsRelevant)
                            .Select(card => new ListUserCardsDTO.Card
                            {
                                CardId = card.Id,
                                CardName = card.Name,
                                CardRequirement = card.Requirement,
                                CardTerm = card.Term,
                                SupervisorTermAssessment = card.SupervisorTermAssessment,
                                CountOfComments = card.CountOfComments,
                                CountOfFiles = card.CountOfFiles
                            }).ToList()
                    }).ToList()
                };

                return new BaseResponse<ListUserCardsDTO>()
                {
                    Data = model,
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<ListUserCardsDTO>()
                {
                    Description = $"[ListUserCards] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<ListUserArchivedCardsDTO>> ListUserArchivedCards(int userId)
        {
            try
            {
                var user = await _unitOfWork.Users.GetAsync(x => x.Id == userId, includeProperties: "Columns.Cards");
                if (user is null)
                {
                    return new BaseResponse<ListUserArchivedCardsDTO>()
                    {
                        Description = "Cant find user",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var userArchivedCards = await _unitOfWork.Cards.GetAllAsync(x => x.UserId == userId && x.ReadyToReport);
                var userDeletedCards = await _unitOfWork.Cards.GetAllAsync(x => x.UserId == userId && x.IsDeleted);

                var model = new ListUserArchivedCardsDTO
                {
                    UserId = user.Id,
                    UserName = user.Name,
                    SspName = user.SspName,
                    UserImagePath = user.ImagePath ?? _defaultImagePath,
                    ArchivedCards = (await Task.WhenAll(userArchivedCards
                        .Select(async card => new ListUserArchivedCardsDTO.Card
                        {
                            CardId = card.Id,
                            CardName = card.Name,
                            CardRequirement = card.Requirement,
                            CardTerm = card.Term,
                            FactTerm = card.FactTerm ?? card.Term,
                            HoursOfWork = card.HoursOfWork,
                            EmployeeQualityAssessment = (await _unitOfWork.AssessmentQualityResults.GetAsync(x => x.Id == card.EmployeeQualityAssessment))?.Value,
                            EmployeeTermAssessment = (await _unitOfWork.AssessmentTermResults.GetAsync(x => x.Id == card.EmployeeTermAssessment))?.Value,
                            SupervisorQualityAssessment = (await _unitOfWork.AssessmentQualityResults.GetAsync(x => x.Id == card.SupervisorQualityAssessment))?.Value,
                            SupervisorTermAssessment = (await _unitOfWork.AssessmentTermResults.GetAsync(x => x.Id == card.SupervisorTermAssessment))?.Value,
                            CountOfComments = card.CountOfComments,
                            CountOfFiles = card.CountOfFiles
                        }))).ToList(),
                };

                return new BaseResponse<ListUserArchivedCardsDTO>()
                {
                    Data = model,
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<ListUserArchivedCardsDTO>()
                {
                    Description = $"[ListUserArchivedCards] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        //public async Task<IBaseResponse<string>> GetReport(string supervisorName, DateTime startDate, DateTime endDate, bool isAuthenticated)
        //{
        //    try
        //    {
        //        var workbook = new XLWorkbook();

        //        while (startDate <= endDate)
        //        {
        //            var month = startDate.Month;

        //            var wsName = Term.GetMonthName(month) + " " + startDate.Year;
        //            workbook.AddWorksheet(wsName);
        //            var ws = workbook.Worksheet(wsName);

        //            int row = 1;

        //            ws.Cell("A" + row.ToString()).Value = "Наименование ССП";
        //            ws.Cell("A" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("B" + row.ToString()).Value = "ФИО работника";
        //            ws.Cell("B" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("C" + row.ToString()).Value = "Должность";
        //            ws.Cell("C" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("D" + row.ToString()).Value = "Непосредственный руководитель";
        //            ws.Cell("D" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("E" + row.ToString()).Value = "#";
        //            ws.Cell("E" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("F" + row.ToString()).Value = "Наименование SMART-задачи";
        //            ws.Cell("F" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("G" + row.ToString()).Value = "Требование к задаче (что считается исполнением задачи)";
        //            ws.Cell("G" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("H" + row.ToString()).Value = "Плановый срок реализации задачи";
        //            ws.Cell("H" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("I" + row.ToString()).Value = "Оценочное суждение работника";
        //            ws.Cell("I" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("J" + row.ToString()).Value = "Комментарий работника";
        //            ws.Cell("J" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("K" + row.ToString()).Value = "Оценочное суждение непосредственного руководителя";
        //            ws.Cell("K" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("L" + row.ToString()).Value = "Комментарий непосредственного руководителя";
        //            ws.Cell("L" + row.ToString()).Style.Font.Bold = true;

        //            ws.Cell("M" + row.ToString()).Value = "% выполнения задачи в зависимости от присвоенного оценочного суждения непосредственного руководителя";
        //            ws.Cell("M" + row.ToString()).Style.Font.Bold = true;

        //            row++;

        //            List<User> employees = new List<User>();
        //            if (isAuthenticated)
        //            {
        //                employees = await _repository.UserRepository.GetByCondition(x => x.SupervisorName == supervisorName, false);
        //            }
        //            else
        //            {
        //                employees = await _repository.UserRepository.GetAllUsers(false);
        //            }

        //            foreach (var employee in employees)
        //            {
        //                var cards = await _repository.CardRepository.GetUserCards(false, employee.Id);

        //                foreach (var card in cards.Where(x => x.ReadyToReport
        //                && (Term.IsEqualQuarter(startDate, x.Term) || Term.IsEqualQuarter(startDate, x.StartTerm))))
        //                {
        //                    ws.Cell("A" + row.ToString()).Value = "-";
        //                    if (employee.SspName != null)
        //                    {
        //                        ws.Cell("A" + row.ToString()).Value = employee.SspName.ToString();
        //                    }

        //                    ws.Cell("B" + row.ToString()).Value = "-";
        //                    if (employee.Name != null)
        //                    {
        //                        ws.Cell("B" + row.ToString()).Value = employee.Name.ToString();
        //                    }

        //                    ws.Cell("C" + row.ToString()).Value = "-";
        //                    if (employee.Position != null)
        //                    {
        //                        ws.Cell("C" + row.ToString()).Value = employee.Position.ToString();
        //                    }

        //                    ws.Cell("D" + row.ToString()).Value = "-";
        //                    if (employee.SupervisorName != null)
        //                    {
        //                        ws.Cell("D" + row.ToString()).Value = employee.SupervisorName.ToString();
        //                    }

        //                    ws.Cell("E" + row.ToString()).Value = "-";
        //                    if (card.Name != null)
        //                    {
        //                        ws.Cell("E" + row.ToString()).Value = card.Name.ToString();
        //                    }

        //                    ws.Cell("F" + row.ToString()).Value = "-";
        //                    if (card.Requirement != null)
        //                    {
        //                        ws.Cell("F" + row.ToString()).Value = card.Requirement.ToString();
        //                    }

        //                    ws.Cell("G" + row.ToString()).Value = card.StartTerm.ToString();

        //                    ws.Cell("H" + row.ToString()).Value = "-";
        //                    if (AssessmentList.GetAssessments().FirstOrDefault(x => x.Id == card.EmployeeAssessment) != null)
        //                    {
        //                        ws.Cell("H" + row.ToString()).Value = AssessmentList.GetAssessments().First(x => x.Id == card.EmployeeAssessment).Text;
        //                    }

        //                    ws.Cell("I" + row.ToString()).Value = "-";
        //                    if (card.EmployeeComment != null)
        //                    {
        //                        ws.Cell("I" + row.ToString()).Value = card.EmployeeComment.ToString();
        //                    }

        //                    ws.Cell("J" + row.ToString()).Value = "-";
        //                    if (AssessmentList.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment) != null)
        //                    {
        //                        ws.Cell("J" + row.ToString()).Value = AssessmentList.GetAssessments().First(x => x.Id == card.SupervisorAssessment).Text;
        //                    }

        //                    ws.Cell("K" + row.ToString()).Value = "-";
        //                    if (card.SupervisorComment != null)
        //                    {
        //                        ws.Cell("K" + row.ToString()).Value = card.SupervisorComment.ToString();
        //                    }

        //                    //Выставление баллов
        //                    if (card.SupervisorAssessment == 1 ||
        //                        card.SupervisorAssessment == 5 ||
        //                        card.SupervisorAssessment == 6)
        //                    {
        //                        if (card.Term.Month > month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "-";
        //                        }

        //                        else if (card.Term.Month == month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = AssessmentList.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
        //                        }

        //                        else
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "-";
        //                        }
        //                    }

        //                    else if (card.SupervisorAssessment == 2 ||
        //                            card.SupervisorAssessment == 3 ||
        //                            card.SupervisorAssessment == 4)
        //                    {
        //                        if (card.FactTerm.Value.Month > month && card.Term.Month <= month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "0%";
        //                        }

        //                        else if (card.FactTerm.Value.Month > month && card.Term.Month > month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "-";
        //                        }

        //                        else if (card.FactTerm.Value.Month == month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = AssessmentList.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
        //                        }

        //                        else
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "-";
        //                        }
        //                    }

        //                    else if (card.SupervisorAssessment == 7)
        //                    {
        //                        if (card.StartTerm.Month > month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "-";
        //                        }

        //                        else if (card.StartTerm.Month == month)
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "0%";
        //                        }

        //                        else
        //                        {
        //                            ws.Cell("L" + row.ToString()).Value = "0%";
        //                        }
        //                    }

        //                    else if (card.SupervisorAssessment == 8
        //                        || card.SupervisorAssessment == 9)
        //                    {
        //                        ws.Cell("L" + row.ToString()).Value = AssessmentList.GetAssessments().FirstOrDefault(x => x.Id == card.SupervisorAssessment).Value;
        //                    }
        //                    row++;
        //                }

        //                ws.Columns().AdjustToContents();
        //            }

        //            startDate = startDate.AddMonths(1);
        //        }

        //        string prefixPath = "../TrelloClone/wwwroot/";
        //        string savePath = "с "
        //            + startDate.Year.ToString() + "-"
        //            + startDate.Month.ToString() + "-"
        //            + startDate.Day.ToString() + " по "
        //            + endDate.Year.ToString() + "-"
        //            + endDate.Month.ToString() + "-"
        //            + endDate.Day.ToString() + ".xlsx";
        //        workbook.SaveAs(prefixPath + savePath);

        //        return new BaseResponse<string>()
        //        {
        //            Data = savePath,
        //            StatusCode = StatusCodes.OK
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new BaseResponse<string>()
        //        {
        //            Description = $"[GetReport] : {ex.Message}",
        //            StatusCode = StatusCodes.InternalServerError,
        //        };
        //    }
        //}

       
    }
}
