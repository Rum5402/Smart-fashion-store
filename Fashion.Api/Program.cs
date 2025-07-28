using Fashion.Api.Hubs;
using Fashion.Api.Services;
using Fashion.Api.Middlewares;
using Fashion.Contract.Interface;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Fashion.Infrastructure.UnitOfWork;
using Fashion.Service.Authentications;
using Fashion.Service.FittingRoomServices;
using Fashion.Service.Items;
using Fashion.Service.Notifications;
using Fashion.Service.JWT;
using Fashion.Service.Wishlist;
using Fashion.Service.Admin;
using Fashion.Service.Store;
using Fashion.Service.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Fashion.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Fashion API", 
                    Version = "v1",
                    Description = "API for Fashion Application with JWT Authentication"
                });

                // Add JWT Authentication to Swagger
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            // Add SignalR
            builder.Services.AddSignalR();

            // Add HttpClient
            builder.Services.AddHttpClient();

            // Add Entity Framework
            builder.Services.AddDbContext<FashionDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add JWT Authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JwtSettings:Audience"],
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Add Authorization with custom policies
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("CustomerPolicy", policy => 
                    policy.RequireRole("Customer", "Guest", "Explore"));
                options.AddPolicy("ManagerPolicy", policy => 
                    policy.RequireRole("Manager"));
                options.AddPolicy("AdminPolicy", policy => 
                    policy.RequireRole("Admin"));
                options.AddPolicy("StaffPolicy", policy => 
                    policy.RequireRole("Manager", "Admin"));
                options.AddPolicy("TeamMemberPolicy", policy => 
                    policy.RequireRole("TeamMember"));
            });

            // Add Memory Cache
            builder.Services.AddMemoryCache();

            // Add Health Checks
            builder.Services.AddHealthChecks();

            // Add HttpContextAccessor for domain-based store identification
            builder.Services.AddHttpContextAccessor();

            // Add Store Context Service
            builder.Services.AddScoped<IStoreContextService, StoreContextService>();

            // Add Services
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<JwtService>();
            builder.Services.AddScoped<PasswordHasher>();
            
            // Add Global Exception Handler
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();
            
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IItemService, ItemService>();
            builder.Services.AddScoped<IFittingRoomService, FittingRoomService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationHub, NotificationHubService>();
            builder.Services.AddScoped<IWishlistService, WishlistService>();
            builder.Services.AddScoped<IStoreManagementService, StoreManagementService>();
            builder.Services.AddScoped<IStoreControlService, StoreControlService>();
            builder.Services.AddScoped<IProductFilterService, ProductFilterService>();
            builder.Services.AddScoped<IFittingRoomManagementService, FittingRoomManagementService>();
            builder.Services.AddScoped<IHomeService, HomeService>();
            builder.Services.AddScoped<IManagerProfileService, ManagerProfileService>();
            builder.Services.AddScoped<IFittingRoomRequestService, FittingRoomRequestService>();
            builder.Services.AddScoped<Fashion.Service.Common.ICacheService, Fashion.Service.Common.CacheService>();
            builder.Services.AddScoped<ITeamMemberService, TeamMemberService>();
            builder.Services.AddScoped<ITeamMemberFittingRoomService, TeamMemberFittingRoomService>();

            // Add CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
                
                options.AddPolicy("AllowSpecific", policy =>
                {
                    policy.WithOrigins(
                            "http://localhost:3000", // React default
                            "http://localhost:4200", // Angular default
                            "http://localhost:8080", // Vue default
                            "http://127.0.0.1:3000",
                            "http://127.0.0.1:4200",
                            "http://127.0.0.1:8080",
                            "https://localhost:3000",
                            "https://localhost:4200",
                            "https://localhost:8080",
                            "https://127.0.0.1:3000",
                            "https://127.0.0.1:4200",
                            "https://127.0.0.1:8080"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Add StoreDomainMiddleware before authentication
            app.UseMiddleware<StoreDomainMiddleware>();

            app.UseCors("AllowSpecific");

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseAdminAuthorization();
            app.UseWishlistAuthorization();
            
            // Add global exception handler
            app.UseExceptionHandler();

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");
            
            // Add health check endpoint
            app.MapHealthChecks("/health");

            app.Run();
        }
    }
} 