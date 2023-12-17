using WebApi.Configuration;
using WebApi.Services;
using DatabaseAccessLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ConnectionStringsOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<JwtAuthenticationOptions>(builder.Configuration.GetSection("JwtAuthentication"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please insert JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddDbContext<CinemaDbContext>(x => x.UseSqlServer(builder.Services.BuildServiceProvider().GetRequiredService<IOptions<ConnectionStringsOptions>>().Value.CinemaDbConnectionString));

builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<MovieService>();
builder.Services.AddTransient<ScreeningService>();
builder.Services.AddTransient<ScreeningPriceService>();
builder.Services.AddTransient<SeatService>();
builder.Services.AddTransient<HallService>();
builder.Services.AddTransient<TicketService>();
builder.Services.AddTransient<CountryService>();
builder.Services.AddTransient<DirectorService>();
builder.Services.AddTransient<GenreService>();
builder.Services.AddTransient<JwtService>();

builder.Services
    .AddAuthentication(options =>
    {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(jwtOptions =>
    {
        var config = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtAuthenticationOptions>>().Value;

        jwtOptions.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = config.Issuer,
            ValidAudience = config.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key)),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();