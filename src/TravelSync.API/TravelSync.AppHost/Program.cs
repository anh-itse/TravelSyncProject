using TravelSync.AppHost.Configurations;
using TravelSync.Application.DependencyInjection.Extensions;
using TravelSync.Infrastructure.DependencyInjection.Extensions;
using TravelSync.Persistence.DependencyInjection.Extensions;
using TravelSync.Persistence.DependencyInjection.Options;
using TravelSync.Presentation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddHttpContextAccessor()
    .AddInfrastructure()
    .AddApplication()
    .AddPersistence(builder.Configuration.GetSection(nameof(SqlServerRetryOptions)));

builder.Services
    .AddControllers()
    .AddApplicationPart(AssemblyReference.Assembly);

// Cấu hình Swagger để hỗ trợ Bearer Token
builder.Services.AddSwaggerWithAuth();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync().ConfigureAwait(false);
