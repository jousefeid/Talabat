using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Repositories;
using Talabat.Errors;
using Talabat.Helpers;
using Talabat.Repository;

namespace Talabat.Extentions
{
    public static class ApilcationServicesExtention
    {
        public static IServiceCollection  AddApilcationServices(this IServiceCollection Services)
        {
            //builder.Services.AddScoped<IGenericRepository<Product> , GenericRepository<Product>>();//inject controller for product
            //builder.Services.AddScoped<IGenericRepository<ProductBrand>, GenericRepository<ProductBrand>>();
            //hard (i neet to create for all controllers that i have)

            Services.AddScoped(typeof(IBasketRepository) ,typeof(BasketRepository));
            //generic for all repositories
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddAutoMapper(m => m.AddProfile(new MappingProfiles()));
            Services.AddAutoMapper(typeof(MappingProfiles));

            #region Error Handling
            Services.Configure<ApiBehaviorOptions>(Options =>
            {
                Options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    //modelState => Dic [KeyValuePaire]
                    //Key => Name of parameter
                    //Value => Errors

                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                                              .SelectMany(p => p.Value.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToArray();

                    var ValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationErrorResponse);
                };
            });
            #endregion

            return Services;
        }
    }
}
