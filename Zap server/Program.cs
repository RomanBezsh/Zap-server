using AutoMapper;
using Zap.BLL.Infrastructure;
using Zap.BLL.Interfaces;
using Zap.BLL.MappingProfiles;
using Zap.BLL.Services;
using Zap.DAL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// ========================= SERVICES =========================

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

// ✅ Подключение БД
builder.Services.AddZapContext(
    builder.Configuration.GetConnectionString("DefaultConnection"));

// ✅ Email config
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// ✅ AutoMapper
var mappingConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddSingleton(mappingConfig.CreateMapper());

// ✅ UnitOfWork и сервисы
builder.Services.AddUnitOfWorkService();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IMediaAttachmentService, MediaAttachmentService>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddTransient<IEmailService, EmailService>();

// ✅ Разрешаем фронтенду подключаться
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins(
                "http://localhost:5173",
                "https://localhost:5173"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});


// ========================= APP =========================
builder.Services.AddSwaggerGen(c =>
{
    c.SupportNonNullableReferenceTypes();
    c.MapType<IFormFile>(() => new Microsoft.OpenApi.Models.OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Включаем CORS
app.UseCors("AllowClient");

app.UseAuthorization();

app.MapControllers();

app.Run();