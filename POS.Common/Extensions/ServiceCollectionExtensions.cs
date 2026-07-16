using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using POS.Business.Generators;
using POS.Business.Rules;
using POS.Business.Rules.Base;
using POS.Business.Rules.GanaMax;
using POS.Business.Services;
using POS.Data;
using POS.Data.Contexts;

namespace POS.Common.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPosExpress(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<PosExpressDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUniqueCodeGenerator, UniqueCodeGenerator>();

        services.AddScoped<IProductAppService, ProductAppService>();
        services.AddScoped<IBarCodeAppService, BarCodeAppService>();
        services.AddScoped<ICategoryAppService, CategoryAppService>();
        services.AddScoped<ISaleAppService, SaleAppService>();

        var strategy = Enum.Parse<BusinessStrategy>(configuration["BusinessStrategy"] ?? nameof(BusinessStrategy.Base));
        if (strategy == BusinessStrategy.GanaMax)
        {
            services.AddScoped<IPriceRule, GanaMaxPriceRule>();
            services.AddScoped<IDiscountRule, GanaMaxDiscountRule>();
            services.AddScoped<IStockRule, GanaMaxStockRule>();
        }
        else
        {
            services.AddScoped<IPriceRule, BasePriceRule>();
            services.AddScoped<IDiscountRule, BaseDiscountRule>();
            services.AddScoped<IStockRule, BaseStockRule>();
        }

        return services;
    }
}
