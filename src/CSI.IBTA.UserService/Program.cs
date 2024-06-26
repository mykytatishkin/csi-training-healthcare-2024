using CSI.IBTA.DataLayer;
namespace CSI.IBTA.UserService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddLogging();

            var connectionString = builder.Configuration.GetConnectionString("UserDBConnection")
                ?? throw new Exception("Connection string is null");
            var connectionString2 = builder.Configuration.GetConnectionString("BenefitsDBConnection")
                ?? throw new Exception("Connection string is null");

            builder.Services.AddUserService(builder.Configuration);
            builder.Services.AddUserUnitOfWork(connectionString);
            builder.Services.AddBenefitsUnitOfWork(connectionString2);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}