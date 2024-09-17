using App.Core.Auth.Constants;
using App.Core.Auth.Dtos;
using App.Core.Auth.Identity.Interface;
using App.Core.Auth.Models;
using App.Core.Auth.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace App.Core.Auth.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IJwtProvider _jwtProviders;

        public AuthRepository(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IJwtProvider jwtProviders)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtProviders = jwtProviders;
        }
        
        public async Task<ResponseDto> Register(RegistrationRequestDto registerDto)
        {
            ApplicationUser user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                PhoneNumber = registerDto.PhoneNumber,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (result.Succeeded)
            {
                registerDto.Role = AppCoreAuthRoles.UserRole;
                var roles = await AssignRole(user.Email, registerDto.Role);
                return new ResponseDto
                {
                    Message = "User was created",
                    IsSuccess = true
                };
            }

            return new ResponseDto
            {
                Message = result.Errors.First().Description,
                IsSuccess = false
            };
        }

        public async Task<ResponseDto> Login(LoginRequestDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var token = _jwtProviders.GenerateJwtToken(user, roles);

                return new ResponseDto
                {
                    Result = new
                    {
                        Id = user.Id,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Roles = string.Join(",", roles),
                        Token = token
                    },
                    IsSuccess = true,
                    Message = null
                };
            }

            return new ResponseDto();
        }

        public async Task<IList<string>> AssignRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }

            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                await CreateRole(roleName);
            }

            await _userManager.AddToRoleAsync(user, roleName);
            return await _userManager.GetRolesAsync(user);
        }
        
        private async Task CreateRole(string roleName)
        {
            if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
