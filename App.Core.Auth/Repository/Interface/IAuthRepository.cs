using App.Core.Auth.Dtos;

namespace App.Core.Auth.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<ResponseDto> Register(RegistrationRequestDto registerDto);
        Task<ResponseDto> Login(LoginRequestDto loginDto);
        Task<IList<string>> AssignRole(string email, string roleName);
    }
}
