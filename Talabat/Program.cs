using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositories;
using Talabat.Errors;
using Talabat.Extentions;
using Talabat.Helpers;
using Talabat.Middlewares;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            #region Configure Services - Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddDbContext<StoreContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<AppIdentityDbContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(Options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");

                return ConnectionMultiplexer.Connect(Connection);
            });

            builder.Services.AddApilcationServices();

            builder.Services.AddIdentityServices();

            #endregion

            var app = builder.Build();

            #region Update Database

            //StoreContext dbContext = new StoreContext();//invalid
            //await dbContext.Database.MigrateAsync();


            using var Scope = app.Services.CreateScope();
            //Group Of Services that lifetime scoped
            var Services = Scope.ServiceProvider;
            //Services itself

            var LoggerFactor = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = Services.GetRequiredService<StoreContext>();
                //Ask CLR for Creating Object from DbContext Expicitly
                await dbContext.Database.MigrateAsync(); // Update Database

                var IdentityDbContext = Services.GetRequiredService<AppIdentityDbContext>();
                await IdentityDbContext.Database.MigrateAsync();

                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);

                await StoreContextSeed.SeedAsync(dbContext);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactor.CreateLogger<Program>();
                Logger.LogError(ex, "Error occured during applying migration (update-database)");
            }
            


            #endregion

            #region Configure - Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleware>();
                app.UseSwaggerMiddleware();
            }

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            #endregion



            app.Run();
        }
    }
}