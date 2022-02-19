using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Users;
using StepEbay.Main.Common.Models.Auth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

using BC = BCrypt.Net.BCrypt;

namespace StepEbay.Main.Api.Common.Services.AuthServices
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserDbService _userDbService;
        public AuthService(IConfiguration config, IUserDbService userDbService)
        {
            _config = config;
            _userDbService = userDbService;
        }

        public async Task<ResponseData<SignInResponseDto>> SignIn(SignInRequestDto request)
        {
            return null;
        }

        public async Task<ResponseData<SignInResponseDto>> SignUp(SignUpRequestDto request)
        {
            //Вова тут тобі тре добавити валідацію даних перед орцією перевіркою на нік нейм (для повернення помилкі на клієнт використовуй оце як приклад
            //ResponseData<SignInResponseDto>.Fail("Нік нейм", "Користувач з таким нікнеймом вже існує!" ))

            if (await _userDbService.AnyByNickName(request.NickName))
                return ResponseData<SignInResponseDto>.Fail("Нік нейм", "Користувач з таким нікнеймом вже існує!" );

            await _userDbService.Add(new User
            {
                NickName = request.NickName,
                FullName = request.FullName,
                Email = request.Email,
                Created = DateTime.UtcNow,
                Password = BC.HashPassword(request.Password)
            });

            return new ResponseData<SignInResponseDto>
            {
                Data = (await SignIn(new SignInRequestDto { NickName = request.NickName, Password = request.Password })).Data
            };
        }

        public async Task<ResponseData<RefreshTokenResponseDto>> RefreshToken(RefreshTokenRequestDto request)
        {
            return null;
        }

        private string GenerateJWT(string username, int expiresIn, List<string> roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, username),
                new Claim("nickName", "")
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(expiresIn),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                ValidateLifetime = false
            };

            //todo
            return null;
        }

        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[32];

            using RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
