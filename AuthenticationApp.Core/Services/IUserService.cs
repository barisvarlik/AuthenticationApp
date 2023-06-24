using AuthenticationApp.Core.DTOs;
using AuthenticationApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApp.Core.Services
{
    public interface IUserService
    {
        Task<ResponseDto<UserDto>> Register(RegistrationDto registerInfo);
        Task<ResponseDto<string>> Login(UserCredentialsDto userCredentials);
        Task<ResponseDto<string>> ChangePassword(ChangePasswordDto userCredentials);
        ResponseDto<NoContentDto> Logout(string token);
        Task<ResponseDto<NoContentDto>> DeleteAccount(User user, string token);
        string CreateToken(User user);
        void DestroyToken(string token);
        void CreatePasswordHash(string password, out byte[] hash, out byte[] salt);
        bool VerifyPasswordHash(string password, byte[] hash, byte[] salt);
    }
}
