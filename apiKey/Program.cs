using System.Text;
using api_key_Authorize.Data;
using api_key_Authorize.Repository;
using apiKey.Middlewares;
using masterapi.Models;
using masterapi.Repositories.TokenRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

// Add services to the container.

// Register the AuthorizationHandler
builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ApiKeyOrJwtHandler>(); // Register your custom handler



// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyOnly", policy =>
        policy.Requirements.Add(new ApiKeyRequirement())); // Create the policy that uses ApiKeyRequirement

    options.AddPolicy("JwtOrApiKey", policy =>
        policy.Requirements.Add(new ApiKeyOrJwtRequirement())); // Define the policy
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Define the API Key security scheme for Swagger UI
    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Description = "Enter your API Key"
    });

    // Define the JWT Bearer security scheme for Swagger UI
    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Bearer Token"
    });

    // Apply the API Key and JWT security requirement globally to all endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            new string[] { }
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            },
            new string[] { }
        }
    });

    // Optionally, you can define multiple Swagger Docs for different API versions
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Master API",
        Version = "v1"
    });
});
// Register repositories
builder.Services.AddScoped<IDbRepository, DbRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();



builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Rahul")
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});

// JWT Authentication setup
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
     .AddJwtBearer(options =>
     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidIssuer = builder.Configuration["Jwt:Issuer"],
         ValidAudience = builder.Configuration["Jwt:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(
             Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
     });


var app = builder.Build();
//app.UseMiddleware<JwtAuthenticationMiddleware>();


// Seed Admin User
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await DataSeeder.SeedAdminUser(services);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error seeding admin user: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication(); // Add authentication middleware

app.UseAuthorization();

app.MapControllers();

app.Run();
