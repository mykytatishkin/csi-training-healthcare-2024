using CSI.IBTA.UserService.Interfaces;
using CSI.IBTA.UserService.Services;
using CSI.IBTA.DataLayer;
using CSI.IBTA.AuthService.Interfaces;
using CSI.IBTA.AuthService.Authentication;

namespace CSI.IBTA.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IUsersService, UsersService>();
            builder.Services.AddScoped<IEmployersService, EmployersService>();

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string is null");

            builder.Services.AddDataLayer(connectionString);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}