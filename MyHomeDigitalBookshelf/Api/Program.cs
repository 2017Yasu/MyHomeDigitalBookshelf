using MyHomeDigitalBookshelf.Application;
using MyHomeDigitalBookshelf.Infrastructure;
using MyHomeDigitalBookshelf.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddInfrastructureServices(
        builder.Configuration.GetSection("Database").Get<DbSettings>()?.ToInfrastructureDbSettings()
            ?? throw new InvalidOperationException("Database settings are not configured properly.")
    )
    .AddApplicationServices();

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

app.UseStaticFiles();
app.UseSpa(spa => { });

app.Run();
