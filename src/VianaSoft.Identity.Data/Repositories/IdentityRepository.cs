using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VianaSoft.BuildingBlocks.Core.Configuration;
using VianaSoft.BuildingBlocks.Core.Notifications.Enumerators;
using VianaSoft.BuildingBlocks.Core.Notifications.Interfaces;
using VianaSoft.BuildingBlocks.Core.Resources.Interfaces;
using VianaSoft.BuildingBlocks.Core.User.Dto.Request;
using VianaSoft.BuildingBlocks.Core.User.Dto.Response;
using VianaSoft.BuildingBlocks.Core.User.Interfaces;
using VianaSoft.Identity.Domain.Entities;
using VianaSoft.Identity.Domain.Interfaces;

namespace VianaSoft.Identity.Data.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        #region Properties

        private readonly INotifier _notifier;
        private readonly IAspNetUser _aspNetUser;
        private readonly ISendGridEmail _sendGridEmail;
        private readonly ILanguageMessage _responseMessage;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationSettings _applicationSettings;

        #endregion

        #region Builders
        public IdentityRepository(INotifier notifier,
                                  IAspNetUser aspNetUser,
                                  ISendGridEmail sendGridEmail,
                                  ILanguageMessage responseMessage,
                                  IHttpContextAccessor httpContextAccessor,
                                  SignInManager<ApplicationUser> signInManager,
                                  UserManager<ApplicationUser> userManager,
                                  IOptions<ApplicationSettings> applicationSettings)
        {
            _notifier = notifier;
            _aspNetUser = aspNetUser;
            _sendGridEmail = sendGridEmail;
            _signInManager = signInManager;
            _responseMessage = responseMessage;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _applicationSettings = applicationSettings.Value;

            _notifier.Add(_responseMessage.RequestSuccessfullyReceivedRepository(), false, HttpStatusCode.OK, TypeError.Success);
        }

        #endregion

        #region Public Methods

        public async Task<ProfileResponseDto?> GetProfileAsync()
        {
            var result = await _userManager.FindByIdAsync(_userManager.GetUserId(_httpContextAccessor.HttpContext.User));
            if (result == null)
            {
                _notifier.Add(_responseMessage.NotFound());
                return default;
            }

            return new ProfileResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                Email = result.Email,
                Phone = result.PhoneNumber
            };
        }
        public async Task<UserLoginResponseDto?> LoginAsync(UserLoginRequestDto request)
        {
            var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                    _notifier.Add(_responseMessage.IsLockedOut());
                else if (result.IsNotAllowed)
                    _notifier.Add(_responseMessage.IsNotAllowed());
                else if (result.RequiresTwoFactor)
                    _notifier.Add(_responseMessage.TwoFactor());
                else
                    _notifier.Add(_responseMessage.InvalidPassword());

                return default;
            }

            var user = new ApplicationUser
            {
                UserName = request.Email,
                Email = request.Email
            };

            return await GenerateToken(user);
        }
        public async Task<UserLoginResponseDto?> RegisterUserAsync(UserRegistrationRequestDto request)
        {
            var user = new ApplicationUser
            {
                Name = request.Name,
                UserName = request.Email,
                Email = request.Email,
                EmailConfirmed = true,
                PhoneNumber = request.Phone,
                PhoneNumberConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded && result.Errors.Any())
            {
                NotifyPasswordErrors(result);
                return default;
            }

            await AddClaimAsync(user);
            await _signInManager.SignInAsync(user, false);

            return await GenerateToken(user);
        }
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _notifier.Add(_responseMessage.InvalidEmail(), false);
                return true;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var link = $"{_applicationSettings.SendGridSettings.UrlRedirect}?token={token}&email={user.Email.ToLower()}";

            return await _sendGridEmail.SendPasswordResetEmail(request.Email, "Reset Password", link);
        }
        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _notifier.Add(_responseMessage.NotFound());
                return false;
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);
            if (!result.Succeeded && result.Errors.Any())
            {
                NotifyPasswordErrors(result);
                return false;
            }

            return true;
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _notifier.Add(_responseMessage.NotFound());
                return false;
            }

            var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
            if (!result.Succeeded && result.Errors.Any())
            {
                NotifyPasswordErrors(result);
                return default;
            }

            return true;
        }
        public async Task<bool> IsResetPasswordTokenValidAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _notifier.Add(_responseMessage.NotFound());
                return false;
            }

            var result = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, UserManager<ApplicationUser>.ResetPasswordTokenPurpose, token);
            if (!result)
            {
                _notifier.Add(_responseMessage.InvalidToken());
            }

            return result;
        }
        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region Private Methods
        private async Task<UserLoginResponseDto> GenerateToken(ApplicationUser request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            var claims = await GetClaimsAsync(user);
            var encodedToken = GenerateTokenHandler(claims);

            return GetUserToken(encodedToken, claims.Claims, user);
        }
        private async Task<ClaimsIdentity> GetClaimsAsync(ApplicationUser user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Name, user.Name));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach (var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            return new ClaimsIdentity(claims);
        }
        private string GenerateTokenHandler(ClaimsIdentity claims)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var currentIssuer = $"{_aspNetUser.GetHttpContext().Request.Scheme}://{_aspNetUser.GetHttpContext().Request.Host}";
            var key = Encoding.ASCII.GetBytes(_applicationSettings.ApiSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _applicationSettings.ApiSettings.Issuer,
                Audience = _applicationSettings.ApiSettings.Audience,
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(_applicationSettings.ApiSettings.ExpirationHours),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            });

            return $"Bearer {tokenHandler.WriteToken(token)}";
        }
        private UserLoginResponseDto GetUserToken(string encodedToken, IEnumerable<Claim> claims, ApplicationUser user)
        {
            return new UserLoginResponseDto
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_applicationSettings.ApiSettings.ExpirationHours).TotalSeconds,
                UserToken = new UserTokenResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    UrlImage = user.UrlImage,
                    Claims = claims.Select(x => new UserClaimResponseDto { Type = x.Type, Value = x.Value })
                }
            };
        }
        private async Task AddClaimAsync(ApplicationUser user)
        {
            await _userManager.AddClaimAsync(user, new Claim("BackOffice", "Read"));
            await _userManager.AddClaimAsync(user, new Claim("BackOffice", "Create"));
            await _userManager.AddClaimAsync(user, new Claim("BackOffice", "Update"));
        }
        private void NotifyPasswordErrors(IdentityResult result)
        {
            foreach (var item in result.Errors)
            {
                switch (item.Code)
                {
                    case "PasswordRequiresNonAlphanumeric":
                        _notifier.Add(_responseMessage.PasswordRequiresNonAlphanumeric());
                        break;
                    case "PasswordRequiresUpper":
                        _notifier.Add(_responseMessage.PasswordRequiresUpper());
                        break;
                    case "PasswordRequiresLower":
                        _notifier.Add(_responseMessage.PasswordRequiresLower());
                        break;
                    case "PasswordRequiresDigit":
                        _notifier.Add(_responseMessage.PasswordRequiresDigit());
                        break;
                    case "InvalidToken":
                        _notifier.Add(_responseMessage.InvalidToken());
                        break;
                    default:
                        _notifier.Add(item.Description);
                        break;
                }
            }
        }
        private static long ToUnixEpochDate(DateTime date)
            => (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);

        #endregion
    }
}
