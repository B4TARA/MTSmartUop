using MtSmart.BLL.DTO;
using MtSmart.BLL.DTO.AccountDTOs;
using MtSmart.BLL.Enums;
using MtSmart.BLL.Interfaces;
using MtSmart.DAL.Entities;
using MtSmart.DAL.Interfaces;
using System.Security.Claims;

namespace MtSmart.BLL.Services
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IBaseResponse<ClaimsIdentity>> Login(UserLoginDTO model)
        {
            try
            {
                var user = await _unitOfWork.Users.GetAsync(x => x.Login == model.Login);

                if (user == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден"
                    };
                }

                if (user.Password != model.Password)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль"
                    };
                }

                if (user.IsBlocked)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Учетная запись заблокирована"
                    };
                }

                var result = Authenticate(user);
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    StatusCode = StatusCodes.OK
                };
            }

            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[AccountService.Login] : {ex.Message}",
                    StatusCode = StatusCodes.InternalServerError
                };
            }
        }

        private ClaimsIdentity Authenticate(User user)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.ToString()),
                new Claim("Id", user.Id.ToString())
            };

            if (user.Login == null)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, "Default"));
            }
            else
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login));
            }

            if (user.Name == null)
            {
                claims.Add(new Claim("Name", "Default"));
            }
            else
            {
                claims.Add(new Claim("Name", user.Name));
            }

            if (user.ImagePath == null)
            {
                claims.Add(new Claim("ImagePath", "Default"));
            }
            else
            {
                claims.Add(new Claim("ImagePath", user.ImagePath));
            }

            return new ClaimsIdentity(claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        }

        public async Task<bool> RemindPassword(string email)
        {
            var userToRemindPassword = await _unitOfWork.Users.GetAsync(x => x.Email == email);

            if (userToRemindPassword == null)
            {
                return false; // Пользователя с таким email-ом не существует
            }

            var content = "Ваш логин - " + userToRemindPassword.Login + ". Ваш пароль - " + userToRemindPassword.Password;
            //var message = new Message(new string[] { userToRemindPassword.Email }, "Напоминание", content, userToRemindPassword.Name);
            //await _emailSender.SendEmailAsync(message);

            return true; // Успешно высланы учетные данные на почту
        }
    }
}
