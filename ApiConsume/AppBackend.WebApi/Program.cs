using AppBackend.BusinessLayer.Abstract;
using AppBackend.BusinessLayer.Concrete;
using AppBackend.DataAccessLayer.Abstract;
using AppBackend.DataAccessLayer.Concrete;
using AppBackend.DataAccessLayer.EntityFramework;
using AppBackend.EntityLayer.Concrete.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using randevuburada.EntityLayer.Concrete.Identity;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddIdentity<AppUser, AppRole>(options =>
{
    // options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    // options.Lockout.MaxFailedAccessAttempts = 5;
    // options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        // ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    });

builder.Services.AddAuthorization(Options =>
{
    Options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    Options.AddPolicy("User", policy => policy.RequireRole("User"));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultBufferSize = 50000; // Varsayýlan tampon boyutu
    options.JsonSerializerOptions.MaxDepth = 500; // Nesne derinliði limiti
    options.JsonSerializerOptions.IgnoreNullValues = true; // Null deðerleri yok say
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true; // Özellik adlarýnda büyük-küçük harf duyarlýlýðýný kaldýr
});
builder.Services.AddDbContext<Context>();
builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddScoped<IQuestionsDal, EfQuestionsDal>();
builder.Services.AddScoped<IQuestionsService, QuestionsManager>();

builder.Services.AddScoped<IAnswersDal, EfAnswersDal>();
builder.Services.AddScoped<IAnswersService, AnswersManager>();

builder.Services.AddScoped<IQuestionsCategoriesDal, EfQuestionsCategoriesDal>();
builder.Services.AddScoped<IQuestionsCategoriesService, QuestionsCategoriesManager>();

builder.Services.AddScoped<IFeedbacksDal, EfFeedbacksDal>();
builder.Services.AddScoped<IFeedbacksService, FeedbacksManager>();

builder.Services.AddScoped<IQuestionsSubCategoriesDal, EfQuestionsSubCategoriesDal>();
builder.Services.AddScoped<IQuestionsSubCategoriesService, QuestionsSubCategoriesManager>();

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("HackathonBTKApiCors", opts =>
    {
        opts.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("HackathonBTKApiCors");

app.UseAuthentication();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
