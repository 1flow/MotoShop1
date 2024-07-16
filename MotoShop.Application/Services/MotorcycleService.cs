using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Interfaces.Repositories;
using MotoShop.Domain.Result;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using MotoShop.Application.Resources;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Validations;
using Npgsql.Replication;
using AutoMapper;

namespace MotoShop.Application.Services
{
    public class MotorcycleService : IMotorcycleService
    {
        private readonly IBaseRepository<Motorcycle> _motorcycleRepository;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMotorcycleValidator _motorcycleValidator;

        public MotorcycleService(IBaseRepository<Motorcycle> motorcycleRepository, IMotorcycleValidator motorcycleValidator/*, ILogger logger*/, IMapper mapper)
        {
            _motorcycleRepository = motorcycleRepository;
            _motorcycleValidator = motorcycleValidator;
            _mapper = mapper;
            // _logger = logger;
        }
        /// <inheritdoc/>
        public async Task<BaseResult<MotorcycleDto>> CreateMotorcycleAsync(CreateMotorcycleDto dto)
        {      
            
           var motorcycle = new Motorcycle()
            {
                Name = dto.Name,
                MotorcycleModel = dto.MotorcycleModel,
                MotorcycleType = dto.MotorcycleType,
                Capacity = dto.Capacity,
                Description = dto.Description,
                Price = dto.Price,
                CreatedAt = DateTime.UtcNow
            };

            
            await _motorcycleRepository.CreateAsync(motorcycle);
            return new BaseResult <MotorcycleDto>()
            {
                 Data = _mapper.Map<MotorcycleDto>(motorcycle)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<MotorcycleDto>> DeleteMotorcycleAsync(long id)
        {
            try
            {
                var motorcycle = await _motorcycleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                var result = _motorcycleValidator.ValidteOnNull(motorcycle);
                if (!result.IsSucces)
                {
                    return new BaseResult<MotorcycleDto>
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage
                    };
                }
                _motorcycleRepository.Remove(motorcycle);
                await _motorcycleRepository.SaveChangesAsync();

                return new BaseResult<MotorcycleDto>()
                {
                    Data = _mapper.Map<MotorcycleDto>(motorcycle)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MotorcycleDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }

        }

        /// <inheritdoc/>
        public Task<BaseResult<MotorcycleDto>> GetMotorcycleByIdAsync(long id)
        {
            MotorcycleDto? report;

            try
            {
                report = _motorcycleRepository.GetAll()
                    .AsEnumerable()
                    .Select(x => new MotorcycleDto(x.Id, x.MotorcycleModel, x.Name))
                    .FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return Task.FromResult(new BaseResult<MotorcycleDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                });

            }
            if (report == null)
            {
                _logger.Warning($"мотоцикл с {id} не найден");
                return Task.FromResult(new BaseResult<MotorcycleDto>()
                {
                    ErrorMessage = ErrorMessage.MotorcycleNotFound,
                    ErrorCode = (int?)ErrorCodes.MotorcycleNotFound
                });
            }
            return Task.FromResult(new BaseResult<MotorcycleDto>
            {
                Data = report
            });
        }

        /// <inheritdoc/>
        public async Task<CollectionResult<MotorcycleDto>> GetMotorcyclesAsync()
        {
            MotorcycleDto[] motorcycleDtos;
            try
            {
                motorcycleDtos = await _motorcycleRepository.GetAll()                    
                    .Select(x => new MotorcycleDto(x.Id, x.MotorcycleModel,x.Name))
                    .ToArrayAsync();

               
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new CollectionResult<MotorcycleDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }
            if (!motorcycleDtos.Any())
            {
               // _logger.Warning(ErrorMessage.MotorcyclesNotFound, motorcycleDtos.Length);
                return new CollectionResult<MotorcycleDto>()
                {
                    ErrorMessage = ErrorMessage.MotorcyclesNotFound,
                    ErrorCode = (int?)ErrorCodes.MotorcyclesNotFound
                };
            }
            return new CollectionResult<MotorcycleDto>()
            {
                Count = motorcycleDtos.Length,
                Data = motorcycleDtos
            };
        }

        public async Task<BaseResult<MotorcycleDto>> UpdateMotorcycleAsync(UpdateMotorcycleDto dto)
        {
            try
            {
                var motorcycle = await _motorcycleRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
                var result = _motorcycleValidator.ValidteOnNull(motorcycle);
                if (!result.IsSucces)
                {
                    return new BaseResult<MotorcycleDto>
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage
                    };
                }
                motorcycle.Description = dto.Decription;
                motorcycle.Price = dto.Price;

                var updatedMotorcycle = _motorcycleRepository.Update(motorcycle);
                await _motorcycleRepository.SaveChangesAsync();

                return new BaseResult<MotorcycleDto>()
                {
                    Data = _mapper.Map<MotorcycleDto>(updatedMotorcycle)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<MotorcycleDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }
        }
    }
}
