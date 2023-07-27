using AutoMapper;
using VianaSoft.BuildingBlocks.Core.User.Dto.Request;
using VianaSoft.BuildingBlocks.Core.User.Dto.Response;
using VianaSoft.Identity.App.Models.Request;
using VianaSoft.Identity.App.Models.Response;

namespace VianaSoft.Identity.App.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProfileResponseDto, ProfileResponse>().ReverseMap();
            CreateMap<UserLoginRequestDto, UserLoginRequest>().ReverseMap();
            CreateMap<UserLoginResponseDto, UserLoginResponse>().ReverseMap();
            CreateMap<UserTokenResponseDto, UserTokenResponse>().ReverseMap();
            CreateMap<UserClaimResponseDto, UserClaimResponse>().ReverseMap();
            CreateMap<UserRegistrationRequestDto, UserRegistrationRequest>().ReverseMap();
            CreateMap<ResetPasswordRequestDto, ResetPasswordRequest>().ReverseMap();
            CreateMap<ForgotPasswordRequestDto, ForgotPasswordRequest>().ReverseMap();
            CreateMap<ChangePasswordRequestDto, ChangePasswordRequest>().ReverseMap();
        }
    }
}
