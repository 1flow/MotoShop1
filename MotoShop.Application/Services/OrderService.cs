using AutoMapper;
using MotoShop.Application.Resources;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Entity;
using MotoShop.Domain.Enum;
using MotoShop.Domain.Interfaces.Repositories;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Result;
using Microsoft.EntityFrameworkCore;
using MotoShop.Domain.Dto.Order;
using MotoShop.DAL;
using Serilog;
using MotoShop.Producer.Interfaces;
using Microsoft.Extensions.Options;
using MotoShop.Domain.Settings;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MotoShop.Application.Services
{
    public class OrderSerive : IOrderService
    {
        private readonly IBaseRepository<User> _userRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Motorcycle> _motorcycleRepository;
        private readonly IMotorcycleService _motorcycleService;
        private readonly Serilog.ILogger _logger;
        private readonly IMapper _mapper;        
        private readonly IUserValidator _userValidator;
        private readonly IOrderValidator _orderValidator;
        private readonly ApplicationDbContext _dbContext;
        private readonly IMessageProducer _messageProducer;
        private readonly IOptions<RabbitMqSettings> _rabbitMqOptions ;

        public OrderSerive(ILogger logger, ApplicationDbContext dbContext, IMotorcycleService motorcycleService,
            IBaseRepository<User> userRepository, IBaseRepository<Order> orderRepository,
            IBaseRepository<Motorcycle> motorcycleRepository, IMapper mapper, IUserValidator userValidator,
            IOrderValidator orderValidator, IMessageProducer messageProducer, IOptions<RabbitMqSettings> rabbitMqOptions)
        {
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _motorcycleRepository = motorcycleRepository;
            _userValidator = userValidator;
            _orderValidator = orderValidator;
            _mapper = mapper;
            _dbContext = dbContext;
            _motorcycleService = motorcycleService;
            _logger = logger;
            _messageProducer = messageProducer;
            _rabbitMqOptions = rabbitMqOptions;
        }


        private async Task AttachMotorcycleToOrder(Motorcycle motorcycle, Order order)
        {
            order.motorcycleList.Add(motorcycle);
            _dbContext.Attach<Order>(order);            
            
        }
        /// <inheritdoc/>
        public async Task<BaseResult<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
        {
            var user = await _userRepository.GetAll().FirstOrDefaultAsync(x=>x.Id == dto.User.Id);
            var result =  _userValidator.ValidteOnNull(user);
            
            if (!result.IsSucces)
            {
                return new BaseResult<OrderDto>
                {
                    ErrorCode = result.ErrorCode,
                    ErrorMessage = result.ErrorMessage,
                };
            }
            var order = new Order
            {
                
                UserId = dto.User.Id,
                OrderDate = dto.OrderDate,
                DeliveryAddress = dto.DeliveryAddress,
                motorcycleList = new List<Motorcycle>()

            };

            foreach (var motorcycleDto in dto.MotorcycleList)
            {
                var motorcycle = await _motorcycleRepository.GetAll().AsNoTracking().FirstOrDefaultAsync(x => x.Id == motorcycleDto.Id);
                
                if (motorcycle != null)
                {
                    await AttachMotorcycleToOrder(motorcycle, order);
                }
                else
                {
                    return new BaseResult<OrderDto>()
                    {
                        ErrorMessage = ErrorMessage.MotorcycleNotFound,
                        ErrorCode = (int?)ErrorCodes.MotorcycleNotFound
                    };
                }
            }

            await _orderRepository.CreateAsync(order);
            await _orderRepository.SaveChangesAsync();

            _messageProducer.SendMessage(order, _rabbitMqOptions.Value.RoutingName, _rabbitMqOptions.Value.ExchangeName);

            return new BaseResult<OrderDto>()
            {
                Data = _mapper.Map<OrderDto>(order)
            };
        }

        /// <inheritdoc/>
        public async Task<BaseResult<OrderDto>> DeleteOrderAsync(long id)
        {
            try
            {
                var order = await _orderRepository.GetAll().FirstOrDefaultAsync(x => x.Id == id);
                var result = _orderValidator.ValidteOnNull(order);
                if (!result.IsSucces)
                {
                    return new BaseResult<OrderDto>
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage
                    };
                }
                _orderRepository.Remove(order);
                await _orderRepository.SaveChangesAsync();

                return new BaseResult<OrderDto>()
                {
                    Data = _mapper.Map<OrderDto>(order)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }

        }

        /// <inheritdoc/>
        public Task<BaseResult<OrderDto>> GetOrderByIdAsync(long id)
        {
            OrderDto? order;

            try
            {

                order = _orderRepository.GetAll()
                    .AsEnumerable()
                    .Select(x => new OrderDto(x.Id, x.UserId,x.motorcycleList.Select(m => new MotorcycleDto(m.Id, m.MotorcycleModel, m.Name)).ToList(), x.DeliveryAddress, x.OrderDate))
                    .FirstOrDefault(x => x.Id == id);


            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return Task.FromResult(new BaseResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                });

            }
            if (order == null)
            {
                _logger.Warning($"заказ с {id} не найден");
                return Task.FromResult(new BaseResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.MotorcycleNotFound,
                    ErrorCode = (int?)ErrorCodes.MotorcycleNotFound
                });
            }
            return Task.FromResult(new BaseResult<OrderDto>
            {
                Data = order
            });
        }

        /// <inheritdoc/>
        public async Task<CollectionResult<OrderDto>> GetUsersOrdersAsync(long id)
        {
            OrderDto[]? orderDtos = null;
            try
            {
                var dsadas = await _orderRepository.GetAll()
                    .Where(x=>x.UserId == id)
                    .Select(x => new OrderDto(x.Id, x.UserId,
                        x.motorcycleList.Select(m => new MotorcycleDto(m.Id,m.MotorcycleModel,m.Name)).ToList(),
                        x.DeliveryAddress, x.OrderDate))
                    .ToArrayAsync();


            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new CollectionResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }
            if (!orderDtos.Any())
            {
                 _logger.Warning(ErrorMessage.OrderNotFound, orderDtos.Length);
                return new CollectionResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.OrdersNotFound,
                    ErrorCode = (int?)ErrorCodes.OrdersNotFound
                };
            }
            return new CollectionResult<OrderDto>()
            {
                Count = orderDtos.Length,
                Data = orderDtos
            };
        }

        public async Task<BaseResult<OrderDto>> UpdateOrderAsync(OrderDto dto)
        {
            try
            {
                var order = await _orderRepository.GetAll().FirstOrDefaultAsync(x => x.Id == dto.Id);
                var result = _orderValidator.ValidteOnNull(order);
                if (!result.IsSucces)
                {
                    return new BaseResult<OrderDto>
                    {
                        ErrorCode = result.ErrorCode,
                        ErrorMessage = result.ErrorMessage
                    };
                }
                order.DeliveryAddress = dto.DeliveryAddress;
                

                var updatedOrder = _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
                
                return new BaseResult<OrderDto>()
                {
                    Data = _mapper.Map<OrderDto>(updatedOrder)
                };
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
                return new BaseResult<OrderDto>()
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int?)ErrorCodes.InternalServerError
                };
            }
        }
    }
}
