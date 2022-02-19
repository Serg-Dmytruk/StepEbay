using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using StepEbay.Common.Models.RefitModels;
using StepEbay.Data.Common.Services.AuthDbServices;
using StepEbay.Data.Common.Services.UserDbServices;
using StepEbay.Data.Models.Auth;
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
        private readonly IRefreshTokenDbService _refreshTokenDbService;
        private readonly IRoleDbService _roleDbService;

        private readonly TimeSpan _expires = new(0, 3, 0);
        public AuthService(IConfiguration config,
            IUserDbService userDbService,
            IRefreshTokenDbService refreshTokenDbService,
            IRoleDbService roleDbService)
        {
            _config = config;
            _userDbService = userDbService;
            _refreshTokenDbService = refreshTokenDbService;
            _roleDbService = roleDbService;
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
                return ResponseData<SignInResponseDto>.Fail("Нікнейм", "Користувач з таким нікнеймом вже існує!" );

            if (await _userDbService.AnyByEmail(request.Email))
                return ResponseData<SignInResponseDto>.Fail("Емейл", "Вказаний емейл вже використовуєтсья!");

            User newUser = await _userDbService.Add(new User
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
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(request.AccessToken);

            User user = await _userDbService.Get(int.Parse(principal.Identity.Name));

            if (!await _refreshTokenDbService.Any(user.Id, request.RefreshToken))
                return ResponseData<RefreshTokenResponseDto>.Fail("Токен", "Відмова в авторизації");

            await _refreshTokenDbService.RemoveRefreshToken(user.Id, request.RefreshToken);

            string newRefreshToken = GenerateRefreshToken();
            await _refreshTokenDbService.Add(new RefreshToken { Token = newRefreshToken, UpdateTime = DateTime.UtcNow.Add(_expires), UserId = user.Id });

            return new ResponseData<RefreshTokenResponseDto>()
            {
                Data = new RefreshTokenResponseDto()
                {
                    AccessToken = GenerateJWT(user, (int)_expires.TotalSeconds, await _roleDbService.GetUserRoleNames(user.Id)),
                    RefreshToken = newRefreshToken,
                    ExpiresIn = (int)_expires.TotalSeconds
                }
            };
        }

        private string GenerateJWT(User user, int expiresIn, List<string> roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Id.ToString()),
                new Claim("nickName", user.NickName)
            };

            JwtSecurityToken token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddSeconds(expiresIn),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
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

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            if(securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Невірний токен");
            }

            return principal;
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
