using AutoMapper;
using FonTech.Domain.Dto.User;
using FonTech.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MotoShop.Application.Resources;
using MotoShop.Domain.Dto;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Databases;
using MotoShop.Domain.Interfaces.Repositories;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Result;
using Serilog;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;


namespace MotoShop.Application.Services
{
    internal class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<UserToken> _userTokenRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly ITokenService _tokenService;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        public AuthService(IBaseRepository<User> userRepository, ILogger logger, IBaseRepository<UserToken> userTokenRepository, ITokenService tokenService, IMapper mapper, IBaseRepository<Role> roleRepository, IBaseRepository<UserRole> userRoleRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userTokenRepository = userTokenRepository;
            _tokenService = tokenService;
            _mapper = mapper;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResult<TokenDto>> Login(LoginUserDto dto)
        {
            try
            {
                var user = await _userRepository.GetAll()
                    .Include(x=> x.Roles)
                    .FirstOrDefaultAsync(x => x.Login == dto.Login);
                if (user == null)
                {
                    return new BaseResult<TokenDto>
                    {
                        ErrorCode = (int)ErrorCodes.UserNotFound,
                        ErrorMessage = ErrorMessage.UserNotFound,

                    };
                }

                var isVerifyPassword = IsVerifyPassword(user.Password, dto.Password);

                if (!isVerifyPassword)
                {
                    return new BaseResult<TokenDto>()
                    {
                        ErrorCode = (int)ErrorCodes.PasswordIsWrong,
                        ErrorMessage = ErrorMessage.PasswordIsWrong
                    };
                }

                var userRoles = user.Roles;
                var claims = userRoles.Select(x=> new Claim(ClaimTypes.Role, x.Name)).ToList();
                claims.Add(new Claim(ClaimTypes.Name, user.Login));

                

                var userToken = await _userTokenRepository.GetAll().FirstOrDefaultAsync(x => x.UserId == user.Id);
                var refreshToken = _tokenService.GenerateRefreshToken();
                var accessToken = _tokenService.GenerateAccessToken(claims);


                if (userToken == null)
                {
                    userToken = new UserToken
                    {
                        UserId = user.Id,
                        RefreshToken = refreshToken,
                        RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7),
                    };
                    await _userTokenRepository.CreateAsync(userToken);
                }
                else
                {
                    userToken.RefreshToken = refreshToken;
                    userToken.RefreshTokenExpireTime = DateTime.UtcNow.AddDays(7);

                    _userTokenRepository.Update(userToken);
                    await _userTokenRepository.SaveChangesAsync();
                }
                return new BaseResult<TokenDto>()
                {
                    Data = new TokenDto()
                    {
                        RefreshToken = refreshToken,
                        AccessToken = accessToken
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<TokenDto>()
                {
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                    ErrorMessage = ErrorMessage.InternalServerError
                };
                
            }
        }

        

        public async Task<BaseResult<UserDto>> Register(RegisterUserDto dto)
        {
            

            if (dto.Password != dto.PasswordConfirm)
            {
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.PasswordNotEqualsPasswordConfirm,
                    ErrorCode = (int)ErrorCodes.PasswordNotEqualsPasswordConfirm
                };
            }

            try
            {
               var user = await _userRepository.GetAll().FirstOrDefaultAsync(x=> x.Login == dto.Login);
                if (user != null) 
                {
                    return new BaseResult<UserDto>()
                    {
                        ErrorMessage = ErrorMessage.UserAlreadyExists,
                        ErrorCode = (int)ErrorCodes.UserAlreadyExists
                    };
                }
                var hashedPassword = HashPassword(dto.Password);

                using (var transaction = await _unitOfWork.BeginTransactionAsync())
                {
                    try
                    {
                        user = new User()
                        {
                            Login = dto.Login,
                            Password = hashedPassword
                        };

                        await _unitOfWork.Users.CreateAsync(user);
                        await _unitOfWork.SaveChangesAsync();


                        var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == nameof(Roles.User));
                        if (role == null)
                        {
                            return new BaseResult<UserDto>()
                            {
                                ErrorCode = (int)ErrorCodes.RoleNotFound,
                                ErrorMessage = ErrorMessage.RoleNotFound,
                            };
                        }

                        UserRole userRole = new UserRole
                        {
                            UserId = user.Id,
                            RoleId = role.Id
                        };

                        await _unitOfWork.UserRoles.CreateAsync(userRole);

                        await _unitOfWork.SaveChangesAsync();

                        await transaction.CommitAsync();
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                    }
                }

                return new BaseResult<UserDto>()
                {
                    Data = _mapper.Map<UserDto>(user)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError
                };
            }

            
        }
        private string HashPassword(string password)
        {
            var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool IsVerifyPassword(string userPasswordHash, string userPassword)
        {
            var hash = HashPassword(userPassword);
            return hash == userPasswordHash;
        }
    }
}
