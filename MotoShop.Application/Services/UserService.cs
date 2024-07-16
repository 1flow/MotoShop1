using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Repositories;
using MotoShop.Domain.Result;
using Microsoft.EntityFrameworkCore;
using Serilog;
using MotoShop.Application.Resources;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Validations;
using AutoMapper;
using FonTech.Domain.Dto.User;
using MotoShop.Domain.Dto.User;

namespace MotoShop.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IUserValidator _userValidator;

        public UserService(IBaseRepository<User> userRepository, IUserValidator userValidator/*, ILogger logger*/, IMapper mapper)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _mapper = mapper;
            // _logger = logger;
        }
        /// <inheritdoc/>
        public async Task<BaseResult<UserDto>> CreateUserAsync(RegisterUserDto dto)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Login == dto.Login);
            var result = _userValidator.CreateUserValidator(user);
            if (!result.IsSucces)
            {
                return new BaseResult<UserDto>
                { 
                    ErrorCode = result.ErrorCode,
                    ErrorMessage = result.ErrorMessage
                };

            }
            user = new User()
            {
                Login = dto.Login,
                Password = dto.Password                
            };

            await _userRepository.CreateAsync(user);
            return new BaseResult <UserDto>()
            {
                 Data = _mapper.Map<UserDto>(user)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<UserDto>> DeleteUserAsync(long id)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                var result = _userValidator.ValidteOnNull(user);
                if (!result.IsSucces)
                {
                    return new BaseResult<UserDto>
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage
                    };
                }
                _userRepository.Remove(user);
                await _userRepository.SaveChangesAsync();

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
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }

        }

        /// <inheritdoc/>
        public Task<BaseResult<UserDto>> GetUserByIdAsync(long id)
        {
            UserDto? report;

            try
            {
                report = _userRepository.GetAll()
                    .AsEnumerable()
                    .Select(x => new UserDto(x.Id, x.Login))
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return Task.FromResult(new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                });

            }
            if (report == null)
            {
                _logger.Warning($"мотоцикл с {id} не найден");
                return Task.FromResult(new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.MotorcycleNotFound,
                    ErrorCode = (int?)ErrorCodes.MotorcycleNotFound
                });
            }
            return Task.FromResult(new BaseResult<UserDto>
            {
                Data = report
            });
        }

        /// <inheritdoc/>
        public async Task<CollectionResult<UserDto>> GetUsersAsync()
        {
            UserDto[] motorcycleDtos;
            try
            {
                motorcycleDtos = await _userRepository.GetAll()                    
                    .Select(x => new UserDto(x.Id, x.Login))
                    .ToArrayAsync();

               
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new CollectionResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }
            if (!motorcycleDtos.Any())
            {
               // _logger.Warning(ErrorMessage.MotorcyclesNotFound, motorcycleDtos.Length);
                return new CollectionResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.MotorcyclesNotFound,
                    ErrorCode = (int?)ErrorCodes.MotorcyclesNotFound
                };
            }
            return new CollectionResult<UserDto>()
            {
                Count = motorcycleDtos.Length,
                Data = motorcycleDtos
            };
        }

        public async Task<BaseResult<UserDto>> UpdateUserAsync(UpdateUserDto dto)
        {
            try
            {
                var user = await _userRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
                var result = _userValidator.ValidteOnNull(user);
                if (!result.IsSucces)
                {
                    return new BaseResult<UserDto>
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage
                    };
                }
                user.Password = dto.NewPassword;
                

                var updatedUser = _userRepository.Update(user);
                await _userRepository.SaveChangesAsync();

                return new BaseResult<UserDto>()
                {
                    Data = _mapper.Map<UserDto>(updatedUser)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<UserDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }
        }
    }
}
