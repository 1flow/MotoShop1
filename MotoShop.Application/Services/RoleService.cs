using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MotoShop.Application.Resources;
using MotoShop.Domain.Dto.Order;
using MotoShop.Domain.Dto.Role;
using MotoShop.Domain.Dto.UserRole;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Databases;
using MotoShop.Domain.Interfaces.Repositories;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotoShop.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Role> _roleRepository;
        private readonly IBaseRepository<UserRole> _userRoleRepository;
        private readonly IRoleValidator _roleValidator;
        private readonly IUserValidator _userValidator;
        private readonly IMapper _mapper;

        public RoleService(IBaseRepository<Role> roleRepository, IBaseRepository<User> userRepository, IRoleValidator roleValidator, IMapper mapper, IUserValidator userValidator, IBaseRepository<UserRole> userRoleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _roleValidator = roleValidator;
            _mapper = mapper;
            _userValidator = userValidator;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResult<UserRoleDto>> AddRoleForUserAsync(UserRoleDto roleDto)
        {
            var user = await _userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == roleDto.Login);

            var validateResult = _userValidator.ValidteOnNull(user);
            if (!validateResult.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = validateResult.ErrorCode,
                    ErrorMessage = validateResult.ErrorMessage,
                };
            }

            var roles = user.Roles.Select(x => x.Name).ToList();
            if (roles.All(x => x != roleDto.RoleName))
            {
                var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == roleDto.RoleName);
                if (role == null)
                {
                    return new BaseResult<UserRoleDto>
                    {
                        ErrorCode = validateResult.ErrorCode,
                        ErrorMessage = validateResult.ErrorMessage,
                    };
                }

                UserRole userRole = new UserRole()
                {
                    RoleId = role.Id,
                    UserId = user.Id,

                };

                await _userRoleRepository.CreateAsync(userRole);
                return new BaseResult<UserRoleDto>()
                {
                    Data = new UserRoleDto
                    {
                        Login = user.Login,
                        RoleName = role.Name,
                    }
                };

            }

            return new BaseResult<UserRoleDto>()
            {
                ErrorCode = (int)ErrorCodes.UserWithThatRoleAlreadyExists,
                ErrorMessage = ErrorMessage.UserWithThatRoleAlreadyExists
            };
        }

        public async Task<BaseResult<RoleDto>> CreateRoleAsync(CreateRoleDto roleDto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Name == roleDto.Name);
            var result = _roleValidator.ValidteOnNull(role);

            if (!result.IsSucces)
            {
                return new BaseResult<RoleDto>
                {
                    ErrorCode = result.ErrorCode,
                    ErrorMessage = result.ErrorMessage,
                };
            }

            role = new Role
            {
                Name = roleDto.Name,
            };

            await _roleRepository.CreateAsync(role);
            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role),
            };
        }

        public async Task<BaseResult<RoleDto>> DeleteRoleAsync(long id)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorCode = (int?)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,

                };
            }

            _roleRepository.Remove(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(role)
            };
        }

        public async Task<BaseResult<UserRoleDto>> DeleteRoleForUserAsync(DeleteUserRoleDto roleDto)
        {
            var user = await _userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == roleDto.Login);

            var validateResult = _userValidator.ValidteOnNull(user);
            if (!validateResult.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = validateResult.ErrorCode,
                    ErrorMessage = validateResult.ErrorMessage,
                };
            }
            var role = user.Roles.FirstOrDefault(x => x.Id == roleDto.RoleId);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int?)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,

                };
            }

            var userRole = await _userRoleRepository.GetAll()
                .Where(x => x.RoleId == role.Id)
                .FirstOrDefaultAsync(x => x.UserId == user.Id);

            _userRoleRepository.Remove(userRole);
            await _userRoleRepository.SaveChangesAsync();
            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto
                {
                    Login = roleDto.Login,
                    RoleName = role.Name,
                }
            };
        }

        public async Task<BaseResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto)
        {
            var role = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == roleDto.Id);
            if (role == null)
            {
                return new BaseResult<RoleDto>()
                {
                    ErrorCode = (int?)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,

                };
            }
            role.Name = roleDto.Name;

            var updatedRole = _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync();

            return new BaseResult<RoleDto>
            {
                Data = _mapper.Map<RoleDto>(updatedRole)
            };
        }

        public async Task<BaseResult<UserRoleDto>> UpdateRoleForUserAsync(UpdateUserRoleDto roleDto)
        {
            var user = await _userRepository.GetAll()
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Login == roleDto.Login);

            var validateResult = _userValidator.ValidteOnNull(user);
            if (!validateResult.IsSucces)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = validateResult.ErrorCode,
                    ErrorMessage = validateResult.ErrorMessage,
                };
            }

            var role = user.Roles.FirstOrDefault(x => x.Id == roleDto.OldRoleId);
            if (role == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int?)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,

                };
            }

            var newRoleForUser = await _roleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == roleDto.NewRoleId);

            if (newRoleForUser == null)
            {
                return new BaseResult<UserRoleDto>()
                {
                    ErrorCode = (int?)ErrorCodes.RoleNotFound,
                    ErrorMessage = ErrorMessage.RoleNotFound,
                };
            }

            using (var transaction = await _unitOfWork.BeginTransactionAsync())
            {
                try
                {
                    var userRole = await _unitOfWork.UserRoles.GetAll()
                        .Where(x => x.RoleId == role.Id)
                        .FirstOrDefaultAsync(x => x.UserId == user.Id);

                    _unitOfWork.UserRoles.Remove(userRole);
                    await _unitOfWork.SaveChangesAsync();

                    await transaction.CommitAsync();

                    var newUserRole = new UserRole()
                    {
                        RoleId = newRoleForUser.Id,
                        UserId = user.Id
                    };

                    await _unitOfWork.UserRoles.CreateAsync(newUserRole);
                    await _unitOfWork.SaveChangesAsync();


                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }
            }

            return new BaseResult<UserRoleDto>()
            {
                Data = new UserRoleDto()
                {
                    Login = user.Login,
                    RoleName = newRoleForUser.Name,
                }

            };
        }
    }
}
