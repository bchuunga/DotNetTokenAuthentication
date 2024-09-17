using App.Core.Auth.Dtos;

namespace App.Core.Auth.Service.Interface
{
    public interface IAuthService
    {
        Task<ResponseDto> Register(RegistrationRequestDto registerDto);
        Task<ResponseDto> Login(LoginRequestDto loginDto);
        Task<IList<string>> AssignRole(string email, string roleName);
    }
}
