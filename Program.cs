//using masterapi.CustomMiddleware;
using api_key_Authorize.Repository;
using apiKey.Middlewares;
using masterapi.Data;
using masterapi.Models;
using masterapi.Repositories.TokenRepository;
using masterapi.Repository;
using masterapi.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<StateRepo>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Master PAI", Version = "v1" });

    options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Enter your JWT Bearer Token"
    });

    options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "X-API-KEY",
        Type = SecuritySchemeType.ApiKey,
        Description = "Enter your API Key"
    });



    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()
        },
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
        }
    });
});



builder.Services.AddDbContext<AuthMasterDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));


// Add authorization services
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiKeyOnly", policy =>
        policy.Requirements.Add(new ApiKeyRequirement())); // Create the policy that uses ApiKeyRequirement

});


// Register the AuthorizationHandler
builder.Services.AddScoped<IAuthorizationHandler, ApiKeyHandler>();



//inject repository
builder.Services.AddScoped<CityRepo>();
// Inject repository
builder.Services.AddScoped<CountryRepo>();

builder.Services.AddScoped<VillageRepo>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IDbRepository, DbRepository>();  



builder.Services.AddScoped<DistrictRepo>();

// Register Seed
builder.Services.AddScoped<DatabaseSeeder>();

// Register the encryption service using configuration settings
builder.Services.AddSingleton<EncryptionService>(new EncryptionService(builder.Configuration["EncryptionSettings:SecretKey"]));


builder.Services.AddIdentityCore<IdentityUser>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("Rahul")
    .AddEntityFrameworkStores<AuthMasterDbContext>()
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


// Register the country controller
builder.Services.AddControllers();

// Add CORS if needed
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});



var app = builder.Build();

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
        // Log errors (implement logging as needed)
        Console.WriteLine($"Error seeding admin user: {ex.Message}");
    }
}

// Seed Data
if (args.Length > 0)
{
    var command = args[0].ToLower();

    if (command == "databaseseeder")
    {
        using var scope = app.Services.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();

        await seeder.Run();
        Console.WriteLine("Database seeding completed.");

        return; // Exit the application after seeding
    }
}

//builder.Services.AddScoped<Jwt>(); // Register the filter itself
//builder.Services.AddScoped<IDbRepository, DbRepository>(); // Register the DbRepository

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

// Global exception handling (if necessary)
app.UseExceptionHandler("/Home/Error");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();