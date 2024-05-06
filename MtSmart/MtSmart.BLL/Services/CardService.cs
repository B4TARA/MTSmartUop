using MtSmart.BLL.BusinessModels;
using MtSmart.BLL.DTO;
using MtSmart.BLL.DTO.CardDTOs;
using MtSmart.BLL.Enums;
using MtSmart.BLL.Infrastructure.Configuration;
using MtSmart.BLL.Interfaces;
using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;
using File = MtSmart.DAL.Entities.File;

namespace MtSmart.BLL.Services
{
    public class CardService : ICardService
    {
        private readonly string _fileUploadPath;
        private readonly IUnitOfWork _unitOfWork;

        public CardService(IUnitOfWork unitOfWork, FileSettings fileSettings)
        {
            _unitOfWork = unitOfWork;
            _fileUploadPath = fileSettings.FileUploadPath;
        }



        public async Task<IBaseResponse<object>> CreateCard(CreateCardDTO createCardDTO)
        {
            try
            {
                var user = await _unitOfWork.Users.GetAsync(x => x.Id == createCardDTO.UserId, includeProperties: "Columns");
                if (user is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find user",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var firstColumn = user.Columns.FirstOrDefault();
                if (firstColumn is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find first column",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                firstColumn.Cards.Add(new Card
                {
                    Name = createCardDTO.CardName,
                    Requirement = createCardDTO.CardRequirement,
                    Term = createCardDTO.CardTerm,
                    StartTerm = createCardDTO.CardTerm,
                    IsRelevant = true,
                    CountOfComments = 0,
                    CountOfFiles = 0,
                    UserId = createCardDTO.UserId,
                    Updates = new List<Update> { new Update
                    {
                        Content = "Создал(а) карточку.",
                        UpdaterName = createCardDTO.UpdaterName,
                        UpdaterImagePath = createCardDTO.UpdaterImagePath,
                    }},
                });

                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[CreateCard] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> UpdateCard(UpdateCardDTO updateCardDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == updateCardDTO.CardId, includeProperties: "Updates");
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var columns = await _unitOfWork.Columns.GetAllAsync(x => x.UserId == card.UserId);
                if (!columns.Any())
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find columns",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                // Изменили название задачи
                if (card.Name != updateCardDTO.CardName)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Название\" с \"{card.Name}\" на \"{updateCardDTO.CardName}\"",
                        UpdaterName = updateCardDTO.UpdaterName,
                        UpdaterImagePath = updateCardDTO.UpdaterImagePath,
                    });

                    card.Name = updateCardDTO.CardName;
                }

                // Изменили срок задачи (перенос)
                if (card.Term != updateCardDTO.CardTerm)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Плановый срок реализации\" с \"{card.Term}\" на \"{updateCardDTO.CardTerm}\"",
                        UpdaterName = updateCardDTO.UpdaterName,
                        UpdaterImagePath = updateCardDTO.UpdaterImagePath,
                    });

                    // Перенос на другой месяц
                    if (TermManager.IsEqualMonths(card.Term, updateCardDTO.CardTerm) == false)
                    {
                        card.ColumnId = columns.First(x => x.Number == 3).Id;
                        card.EmployeeQualityAssessment = null;
                        card.EmployeeTermAssessment = null;
                        card.EmployeeComment = null;
                        card.HoursOfWork = null;
                    }

                    card.Term = updateCardDTO.CardTerm;
                }

                // Изменили требования к задаче
                if (card.Requirement != updateCardDTO.CardRequirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Требование к SMART-задаче\" с \"{card.Requirement}\" на \"{updateCardDTO.CardRequirement}\"",
                        UpdaterName = updateCardDTO.UpdaterName,
                        UpdaterImagePath = updateCardDTO.UpdaterImagePath,
                    });

                    card.Requirement = updateCardDTO.CardRequirement;
                }

                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[UpdateCard] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> MoveCard(MoveCardDTO moveCardDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == moveCardDTO.CardId, includeProperties: "Updates");
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var columns = await _unitOfWork.Columns.GetAllAsync(x => x.UserId == card.UserId);
                if (!columns.Any())
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find columns",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                // Изменили название задачи
                if (card.Name != moveCardDTO.CardName)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Название\" с \"{card.Name}\" на \"{moveCardDTO.CardName}\"",
                        UpdaterName = moveCardDTO.UpdaterName,
                        UpdaterImagePath = moveCardDTO.UpdaterImagePath,
                    });

                    card.Name = moveCardDTO.CardName;
                }

                // Изменили срок выполнения задачи (перенос)
                if (card.Term != moveCardDTO.CardTerm)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Плановый срок реализации\" с \"{card.Term}\" на \"{moveCardDTO.CardTerm}\"",
                        UpdaterName = moveCardDTO.UpdaterName,
                        UpdaterImagePath = moveCardDTO.UpdaterImagePath,
                    });

                    // Перенос на другой месяц
                    if (TermManager.IsEqualMonths(card.Term, moveCardDTO.CardTerm) == false)
                    {
                        card.ColumnId = columns.First(x => x.Number == 3).Id;
                        card.EmployeeQualityAssessment = null;
                        card.EmployeeTermAssessment = null;
                        card.EmployeeComment = null;
                        card.HoursOfWork = null;
                    }
                    // Перенос в этом месяце
                    else
                    {
                        card.ColumnId = card.ColumnId + 1;
                    }

                    card.Term = moveCardDTO.CardTerm;
                }
                // Не меняли срок выполнения задачи
                else
                {
                    // Если задача в 3 колонке
                    if (card.ColumnId == columns.First(x => x.Number == 3).Id)
                    {
                        card.ColumnId = card.ColumnId + 1;
                    }

                    // Если задача не в третьей колонке и срок исполнения задачи истекает в этом или следующем месяце
                    else if (TermManager.IsEqualMonths(card.Term, TermManager.GetDate()) || TermManager.IsEqualMonths(card.Term, TermManager.GetDate().AddMonths(1)))
                    {
                        card.ColumnId = card.ColumnId + 1;
                    }

                    // Если задача не в третьей колонке и задача с предыдущего месяца
                    else if (TermManager.IsEqualMonths(card.Term.AddMonths(1), TermManager.GetDate()))
                    {
                        card.ColumnId = card.ColumnId + 2;
                    }
                }

                // Изменили требования к задаче
                if (card.Requirement != moveCardDTO.CardRequirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Требование к SMART-задаче\" с \"{card.Requirement}\" на \"{moveCardDTO.CardRequirement}\"",
                        UpdaterName = moveCardDTO.UpdaterName,
                        UpdaterImagePath = moveCardDTO.UpdaterImagePath,
                    });

                    card.Requirement = moveCardDTO.CardRequirement;
                }

                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[MoveCard] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> RejectCard(RejectCardDTO rejectCardDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == rejectCardDTO.CardId, includeProperties: "Updates");
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var columns = await _unitOfWork.Columns.GetAllAsync(x => x.UserId == card.UserId);
                if (!columns.Any())
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find columns",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                //изменили название задачи
                if (card.Name != rejectCardDTO.CardName)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Название\" с \"{card.Name}\" на \"{rejectCardDTO.CardName}\"",
                        UpdaterName = rejectCardDTO.UpdaterName,
                        UpdaterImagePath = rejectCardDTO.UpdaterImagePath,
                    });

                    card.Name = rejectCardDTO.CardName;
                }

                //изменили срок задачи (перенос)
                if (card.Term != rejectCardDTO.CardTerm)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Плановый срок реализации\" с \"{card.Term}\" на \"{rejectCardDTO.CardTerm}\"",
                        UpdaterName = rejectCardDTO.UpdaterName,
                        UpdaterImagePath = rejectCardDTO.UpdaterImagePath,
                    });

                    card.Term = rejectCardDTO.CardTerm;
                }

                //изменили требования к задаче
                if (card.Requirement != rejectCardDTO.CardRequirement)
                {
                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Изменил(а) \"Требование к SMART-задаче\" с \"{card.Requirement}\" на \"{rejectCardDTO.CardRequirement}\"",
                        UpdaterName = rejectCardDTO.UpdaterName,
                        UpdaterImagePath = rejectCardDTO.UpdaterImagePath,
                    });

                    card.Requirement = rejectCardDTO.CardRequirement;
                }

                card.ColumnId = card.ColumnId - 1;

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    Content = "Отправил(а) задачу на доработку",
                    UpdaterName = rejectCardDTO.UpdaterName,
                    UpdaterImagePath = rejectCardDTO.UpdaterImagePath,
                });

                //Оповещение по почте
                //await _userService.SendNotification(rejectCardDTO.EmployeeId, "Уведомление", Models.Mailing.Mailing.GetMails()[8]);

                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[RejectCard] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }



        public async Task<IBaseResponse<object>> UploadFiles(UploadFileDTO uploadFileDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == uploadFileDTO.CardId, includeProperties: new string[] { "Files", "Updates" });
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                string directoryPath = Path.Combine(_fileUploadPath, $"Card with id {card.Id}");
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                foreach(var FileToUpload in uploadFileDTO.FilesToUpload)
                {
                    string filePath = Path.Combine(directoryPath, FileToUpload.FileName);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await FileToUpload.CopyToAsync(fileStream);
                    }

                    card.Files.Add(new File
                    {
                        Name = FileToUpload.FileName,
                        Size = FileToUpload.Length,
                        Type = FileToUpload.ContentType,
                        Path = filePath,
                        CardId = uploadFileDTO.CardId,
                    });

                    card.Updates.Add(new Update
                    {
                        CardId = card.Id,
                        Content = $"Прикрепил(а) файл: \"{FileToUpload.Name}\"",
                        UpdaterName = uploadFileDTO.UpdaterName,
                        UpdaterImagePath = uploadFileDTO.UpdaterImagePath,
                    });

                    card.CountOfFiles++;
                }
               
                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[UploadFile] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> DeleteFile(DeleteFileDTO deleteFileDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == deleteFileDTO.CardId, includeProperties: new string[] { "Files", "Updates" });
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var fileToDelete = await _unitOfWork.Files.GetAsync(x => x.Id == deleteFileDTO.FileId);
                if (fileToDelete is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find file",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                if (System.IO.File.Exists(fileToDelete.Path))
                {
                    System.IO.File.Delete(fileToDelete.Path);
                    card.Files.Remove(fileToDelete); // Удаляем файл из списка файлов карточки
                }

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    Content = $"Открепил(а) файл : \"{fileToDelete.Name}\"",
                    UpdaterName = deleteFileDTO.UpdaterName,
                    UpdaterImagePath = deleteFileDTO.UpdaterImagePath,
                });

                card.CountOfFiles--;

                _unitOfWork.Cards.Update(card);
                _unitOfWork.Files.Remove(fileToDelete); // Удаляем сущность File из базы данных
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[DeleteFile] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }



        public async Task<IBaseResponse<object>> AddComment(AddCommentDTO addCommentDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == addCommentDTO.CardId, includeProperties: new string[] { "Comments", "Updates" });
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                card.Comments.Add(new Comment
                {
                    CardId = addCommentDTO.CardId,
                    Content = addCommentDTO.Comment,
                    CommenterName = addCommentDTO.UpdaterName,
                    CommenterImagePath = addCommentDTO.UpdaterImagePath,
                });

                card.Updates.Add(new Update
                {
                    CardId = addCommentDTO.CardId,
                    Content = "Добавил(а) комментарий",
                    UpdaterName = addCommentDTO.UpdaterName,
                    UpdaterImagePath = addCommentDTO.UpdaterImagePath,
                });

                card.CountOfComments++;

                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[AddComment] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }



        public async Task<IBaseResponse<object>> SetSupervisorAssessment(SetSupervisorAssessmentDTO setSupervisorAssessmentDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == setSupervisorAssessmentDTO.CardId, includeProperties: "Updates");
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var columns = await _unitOfWork.Columns.GetAllAsync(x => x.UserId == card.UserId);
                if (!columns.Any())
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find columns",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                card.FactTerm = setSupervisorAssessmentDTO.FactTerm;
                card.SupervisorQualityAssessment = setSupervisorAssessmentDTO.SupervisorQualityAssessment;
                card.SupervisorTermAssessment = setSupervisorAssessmentDTO.SupervisorTermAssessment;
                card.SupervisorComment = setSupervisorAssessmentDTO.SupervisorComment;
                card.ReadyToReport = true;
                card.IsRelevant = false;

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    Content = "Выставил(а) оценочное суждение непосредственного руководителя",
                    UpdaterName = setSupervisorAssessmentDTO.UpdaterName,
                    UpdaterImagePath = setSupervisorAssessmentDTO.UpdaterImagePath,
                });             

                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK,
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[SetSupervisorAssessment] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        public async Task<IBaseResponse<object>> SetEmployeeAssessment(SetEmployeeAssessmentDTO setEmployeeAssessmentDTO)
        {
            try
            {
                var card = await _unitOfWork.Cards.GetAsync(x => x.Id == setEmployeeAssessmentDTO.CardId, includeProperties: "Updates");
                if (card is null)
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find card",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                var columns = await _unitOfWork.Columns.GetAllAsync(x => x.UserId == card.UserId);
                if (!columns.Any())
                {
                    return new BaseResponse<object>()
                    {
                        Description = "Cant find columns",
                        StatusCode = StatusCodes.EntityNotFound
                    };
                }

                card.EmployeeQualityAssessment = setEmployeeAssessmentDTO.EmployeeQualityAssessment;
                card.EmployeeTermAssessment = setEmployeeAssessmentDTO.EmployeeTermAssessment;
                card.EmployeeComment = setEmployeeAssessmentDTO.EmployeeComment;
                card.HoursOfWork = setEmployeeAssessmentDTO.HoursOfWork;

                card.Updates.Add(new Update
                {
                    CardId = card.Id,
                    Content = "Выставил(а) оценочное суждение работника",
                    UpdaterName = setEmployeeAssessmentDTO.UpdaterName,
                    UpdaterImagePath = setEmployeeAssessmentDTO.UpdaterImagePath,
                });

                card.ColumnId = card.ColumnId + 1;

                _unitOfWork.Cards.Update(card);
                await _unitOfWork.CommitAsync();

                return new BaseResponse<object>()
                {
                    StatusCode = StatusCodes.OK,
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<object>()
                {
                    Description = $"[SetEmployeeAssessment] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }
    };
}
