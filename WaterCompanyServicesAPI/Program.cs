using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using WaterCompanyServicesAPI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<WaterCompanyDBContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("WCSConnection")
    ));
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var _context = scope.ServiceProvider.GetRequiredService<WaterCompanyDBContext>();
    _context.Database.EnsureCreated();
}

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
