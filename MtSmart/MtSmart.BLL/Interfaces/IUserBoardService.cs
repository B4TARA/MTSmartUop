using MtSmart.BLL.DTO.UserBoardDTOs;

namespace MtSmart.BLL.Interfaces
{
    public interface IUserBoardService
    {
        Task<IBaseResponse<ListUserCardsDTO>> ListUserCards(int userId);

        Task<IBaseResponse<ListUserArchivedCardsDTO>> ListUserArchivedCards(int userId);
    }
}
