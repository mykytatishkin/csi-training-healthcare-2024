using CSI.IBTA.DataLayer;

namespace CSI.IBTA.AuthService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var connectionString1 = builder.Configuration.GetConnectionString("UserDBConnection")
                ?? throw new Exception("Connection string is null");
            var connectionString2 = builder.Configuration.GetConnectionString("BenefitsDBConnection")
                ?? throw new Exception("Connection string is null");

            builder.Services.AddAuthService(builder.Configuration);
            builder.Services.AddDataLayer(connectionString1, connectionString2);

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
