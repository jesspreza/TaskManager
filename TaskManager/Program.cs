using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TaskManager.Api.Middleware;
using TaskManager.Infra.Data.Context;
using TaskManager.Infra.Ioc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureSwagger();
//builder.Services.AddSwaggerGen();

builder.Services.AddLogging();

//Infrastructure configuration
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager.Api");
    });
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseMiddleware<ExceptionMiddleware>();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.MapFallbackToController("index", "Fallback");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
