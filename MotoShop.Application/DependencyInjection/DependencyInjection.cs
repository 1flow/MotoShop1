using FluentValidation;
using FonTech.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MotoShop.Application.Mapping;
using MotoShop.Application.Services;
using MotoShop.Application.Validations;
using MotoShop.Application.Validations.FluentValidations;
using MotoShop.Domain.Dto.Motorcycle;
using MotoShop.Domain.Interfaces.Services;
using MotoShop.Domain.Interfaces.Validations;
using MotoShop.Domain.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MotoShop.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
                  
            services.AddAutoMapper(typeof(MotorcycleMapping));

            var options = configuration.GetSection(nameof(RedisSettings));
            var redisUrl = options["Url"];
            var instanceName = options["instanceName"];

            services.AddStackExchangeRedisCache(redisCacheOptions =>
            {
                redisCacheOptions.Configuration = redisUrl;
                redisCacheOptions.InstanceName = instanceName;
            });

            InitServices(services);
        }

        private static void InitServices(this IServiceCollection services)
        {
            services.AddScoped<IUserValidator, UserValidator>();
            services.AddScoped<IRoleValidator, RoleValidator>();
            services.AddScoped<IMotorcycleValidator, MotorcycleValidator>();
            services.AddScoped<IOrderValidator,OrderValidator>();
            services.AddScoped<IValidator<CreateMotorcycleDto>, CreateMotorcycleValidator>();
            services.AddScoped<IValidator<UpdateMotorcycleDto>, UpdateMotorcycleValidator>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMotorcycleService, MotorcycleService>();
            services.AddScoped<IOrderService, OrderSerive>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IRoleService, RoleService>();

        }
    }
}
