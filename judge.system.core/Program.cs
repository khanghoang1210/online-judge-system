using EmailService;
using judge.system.core.Database;
using judge.system.core.Service.Impls;
using judge.system.core.Service.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

// Dependency Injection
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ILeetCodeService, LeetCodeService>();
builder.Services.AddScoped<IProblemService, ProblemService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IJudgeService, JudgeService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddHttpContextAccessor();

// Email Service
var emailConfig = builder.Configuration.GetSection("EmailConfiguration")
  .Get<EmailConfiguration>();

builder.Services.AddSingleton(emailConfig);

builder.Services.AddDbContext<Context>(
    opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddAutoMapper(typeof(Program));

//Set 5 min as the lifetime for the HttpMessageHandler objects in the pool used for the Catalog Typed Client
builder.Services.AddHttpClient<ILeetCodeService, LeetCodeService>()
    .SetHandlerLifetime(TimeSpan.FromMinutes(5));
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
            ClockSkew = TimeSpan.Zero
        };
        opt.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                string authorizationHeader = context.Request.Headers["Authorization"];
                if (!string.IsNullOrEmpty(authorizationHeader))
                {
                    string token = authorizationHeader.Substring("Bearer ".Length).Trim();
                    context.Token = token;
                }


                return Task.CompletedTask;
            }
        };
    });

var app = builder.Build();
app.UseCors("AllowOrigin");


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
