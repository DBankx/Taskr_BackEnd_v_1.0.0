using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using Taskr.Domain;
using Taskr.Handlers.Task;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Jwt;
using Taskr.Infrastructure.Mail;
using Taskr.Infrastructure.Pagination;
using Taskr.Infrastructure.PhotoService;
using Taskr.Infrastructure.Security;
using Taskr.Infrastructure.Stripe;
using Taskr.MappingProfiles.Job;
using Taskr.Persistance;

namespace Taskr.Api.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            // configuration bindings
                                    var jwtSettings = new JwtSettings(); 
                                    config.Bind(nameof(JwtSettings), jwtSettings); 
                                    services.AddSingleton(jwtSettings);
                                    services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
                                    services.Configure<MailSettings>(config.GetSection("MailSettings"));
            
                                    // Stripe settings
                                    services.Configure<StripeOptions>(options =>
                                    {
                                        options.SecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
                                        options.PublishableKey= Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY");
                                    });

                                    
            
                        services.AddSwaggerGen(c =>
                        {
                            c.SwaggerDoc("v1", new OpenApiInfo {Title = "Taskr_Api", Version = "v1"});
                            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                            {
                                Description = "Jwt authorization header using the bearer scheme",
                                Name = "Authorization",
                                In = ParameterLocation.Header,
                                Type = SecuritySchemeType.ApiKey,
                                Scheme = "Bearer"
                            });
                                        
                            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                            {
                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        },
                                        Scheme = "oauth2",
                                        Name = "Bearer",
                                        In = ParameterLocation.Header,
                                    },
                                    new List<string>()
                                }
                            });
                        });
                        services.AddMediatR(typeof(GetAllJobsHandler).Assembly);
            
                        // CORS
                        services.AddCors(option =>
                        {
                            option.AddDefaultPolicy(builder =>
                            {
                                builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(new string[]{"http://localhost:3001", "http://localhost:3000"}).AllowCredentials();
                            });
                        });
                        
                        // SignalR
                        services.AddSignalR();
                        
                        // AutoMapper
                        services.AddAutoMapper(typeof(JobProfile).Assembly);
                        
                        // EntityFramework
                        services.AddDbContext<DataContext>(options =>
                        {
                            options.UseSqlServer(config.GetConnectionString("DatabaseConnectionString"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
                        });
                        
                        // Adding identity Options
                        var identityCoreBuilder = services.AddIdentityCore<ApplicationUser>();
                        var identityBuilder = new IdentityBuilder(identityCoreBuilder.UserType, identityCoreBuilder.Services);
                        identityBuilder.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();
                        services.AddAuthentication(option =>
                        {
                            option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                            option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                            option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                        }).AddJwtBearer(option =>
                        {
                            option.SaveToken = true;
                            option.TokenValidationParameters = new TokenValidationParameters
                            {
                                ValidateIssuer = false,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                                ValidateIssuerSigningKey = true,
                                ValidateAudience = false,
                                ValidateLifetime = true,
                                RequireExpirationTime = false
                            };
            
                            option.Events = new JwtBearerEvents
                            {
                                OnMessageReceived = context =>
                                {
                                    var accessToken = context.Request.Query["access_token"];
                                    // get the path
                                    var path = context.HttpContext.Request.Path;
                                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notif") || !string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/message"))
                                    {
                                        context.Token = accessToken;
                                    }
            
                                    return Task.CompletedTask;
                                }
                            };
                        });
            
                        // DI services
                        services.AddScoped<IJwtGenerator, JwtGenerator>();
                        services.AddScoped<IUserAccess, UserAccess>();
                         services.AddSingleton<IUriService>(opt =>
                         { 
                             var accessor = opt.GetRequiredService<IHttpContextAccessor>();
                             var request = accessor.HttpContext.Request;
                             var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                             return new UriService(uri);
                         });
                         services.AddScoped<IPhotoService, PhotoService>();
                         services.AddScoped<IQueryProcessor, EfQueryProcessor>();
                         services.AddTransient<IMailService, MailService>();

                         return services;
        }
    }
}