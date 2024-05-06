using MtSmart.BLL.Enums;

namespace MtSmart.BLL.Interfaces
{
    public interface IBaseResponse<T>
    {
        string? Description { get; }
        StatusCodes StatusCode { get; }
        T? Data { get; }
    }
}
