using MtSmart.BLL.DTO.CardDTOs;

namespace MtSmart.BLL.Interfaces
{
    public interface ICardService
    {
        Task<IBaseResponse<object>> CreateCard(CreateCardDTO createCardDTO);

        Task<IBaseResponse<object>> UpdateCard(UpdateCardDTO updateCardDTO);

        Task<IBaseResponse<object>> MoveCard(MoveCardDTO moveCardDTO);
        Task<IBaseResponse<object>> RejectCard(RejectCardDTO rejectCardDTO);


        Task<IBaseResponse<object>> UploadFiles(UploadFileDTO uploadFileDTO);
        Task<IBaseResponse<object>> DeleteFile(DeleteFileDTO deleteFileDTO);


        Task<IBaseResponse<object>> AddComment(AddCommentDTO addCommentDTO);


        Task<IBaseResponse<object>> SetSupervisorAssessment(SetSupervisorAssessmentDTO setSupervisorAssessmentDTO);
        Task<IBaseResponse<object>> SetEmployeeAssessment(SetEmployeeAssessmentDTO setEmployeeAssessmentDTO);
    }
}
