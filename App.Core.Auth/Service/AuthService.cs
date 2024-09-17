using App.Core.Auth.Dtos;
using App.Core.Auth.Repository.Interface;
using App.Core.Auth.Service.Interface;

namespace App.Core.Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<IList<string>> AssignRole(string email, string roleName)
        {
            return await _authRepository.AssignRole(email, roleName);
        }

        public async Task<ResponseDto> Register(RegistrationRequestDto registerDto)
        {
            return await _authRepository.Register(registerDto);
        }

        public async Task<ResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            return await _authRepository.Login(loginRequestDto);
        }

    }
}
