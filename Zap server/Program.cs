using AutoMapper;
using Zap.BLL.Infrastructure;
using Zap.BLL.Interfaces;
using Zap.BLL.MappingProfiles;
using Zap.BLL.Services;
using Zap.DAL.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddZapContext(
    builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
var mappingConfig = new MapperConfiguration(cfg => {
    cfg.AddProfile<MappingProfile>();
});
builder.Services.AddSingleton(mappingConfig.CreateMapper());
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



//проверка

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
