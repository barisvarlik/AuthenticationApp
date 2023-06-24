using AuthenticationApp.Core.DTOs;
using AuthenticationApp.Core.Entities;
using AuthenticationApp.Core.Repositories;
using AuthenticationApp.Core.Services;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AuthenticationApp.Core;
using AuthenticationApp.Core.UnitOfWorks;

namespace AuthenticationApp.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseDto<string>> ChangePassword(ChangePasswordDto userCredentials)
        {
            var user = await _userRepository.GetUserByEmail(userCredentials.Email);
            if (user == null)
                return ResponseDto<string>.Fail(404, "User not found");
            bool verified = VerifyPasswordHash(userCredentials.OldPassword, user.Credentials.PasswordHash, user.Credentials.PasswordSalt);
            if (!verified)
                return ResponseDto<string>.Fail(401, "Wrong credentials");

            CreatePasswordHash(userCredentials.NewPassword, out byte[] newHash, out byte[] newSalt);

            UserCredentials newCred = new()
            {
                Email = userCredentials.Email,
                PasswordHash = newHash,
                PasswordSalt = newSalt
            };

            _userRepository.ChangeCredentials(newCred);
            await _unitOfWork.CommitAsync();

            return ResponseDto<string>.Success(200);
        }

        public void CreatePasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using HMACSHA512 hmac = new();
            salt = hmac.Key;
            hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.FirstName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.secretKey));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var expirationDate = DateTime.Now.AddDays(1);
            var token = new JwtSecurityToken(claims: claims, expires: expirationDate, signingCredentials: signingCredentials);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            if (Constants.InvalidatedTokens != null && Constants.InvalidatedTokens.Contains(jwt))
                return null;

            return jwt;
        }

        public async Task<ResponseDto<NoContentDto>> DeleteAccount(User user, string token)
        {
            _userRepository.Delete(user);
            await _unitOfWork.CommitAsync();
            Logout(token);
            return ResponseDto<NoContentDto>.Success(204);

        }

        public void DestroyToken(string token)
        {
            if (Constants.InvalidatedTokens == null)
                Constants.InvalidatedTokens = new HashSet<string>();

            Constants.InvalidatedTokens.Add(token);
        }

        public async Task<ResponseDto<string>> Login(UserCredentialsDto userCredentials)
        {
            var user = await _userRepository.GetUserByEmail(userCredentials.Email);
            if (user == null)
                return ResponseDto<string>.Fail(404, "User not found");
            bool verified = VerifyPasswordHash(userCredentials.Password, user.Credentials.PasswordHash, user.Credentials.PasswordSalt);
            if (!verified)
                return ResponseDto<string>.Fail(401, "Wrong credentials");
            var token = CreateToken(user);
            if (token == null)
                return ResponseDto<string>.Fail(401, "Token has expired.");

            return ResponseDto<string>.Success(200, token);
        }

        public ResponseDto<NoContentDto> Logout(string token)
        {

            DestroyToken(string.Empty); // Deal with this line
            return ResponseDto<NoContentDto>.Success(200);
        }

        public async Task<ResponseDto<UserDto>> Register(RegistrationDto registerInfo)
        {

            CreatePasswordHash(registerInfo.Password, out byte[] hash, out byte[] salt);
            User userModel = new()
            {
                FirstName = registerInfo.FirstName,
                LastName = registerInfo.LastName,
                Credentials = new UserCredentials
                {
                    Email = registerInfo.Email,
                    PasswordHash = hash,
                    PasswordSalt = salt
                }
            };
            await _userRepository.AddAsync(userModel);
            await _unitOfWork.CommitAsync();
            var userDto = _mapper.Map<User, UserDto>(userModel);

            return ResponseDto<UserDto>.Success(200, userDto);
        }

        public bool VerifyPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using HMACSHA512 hmac = new(salt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(hash);
        }
    }
}
