using CSI.IBTA.ProcessingService;
using CSI.IBTA.DataLayer;
using CSI.IBTA.ProcessingService.Interfaces;
using CSI.IBTA.ProcessingService.Services;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();
builder.Services.AddScoped<IScopedProcessingService, ScopedProcessingService>();

var connectionString = builder.Configuration.GetConnectionString("BenefitsDBConnection")
    ?? throw new Exception("Connection string is null");
builder.Services.AddBenefitsUnitOfWork(connectionString);

var host = builder.Build();
host.Run();