using Zap.BLL.Infrastructure;
using Zap.BLL.Interfaces;
using Zap.BLL.Services;
using Zap.DAL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddZapContext(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddUnitOfWorkService();

builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

