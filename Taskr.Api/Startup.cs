using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Taskr.Domain;
using Taskr.Handlers.Task;
using Taskr.Infrastructure.Helpers;
using Taskr.Infrastructure.Jwt;
using Taskr.Infrastructure.Mail;
using Taskr.Infrastructure.Middlewares;
using Taskr.Infrastructure.Pagination;
using Taskr.Infrastructure.PhotoService;
using Taskr.Infrastructure.Security;
using Taskr.Infrastructure.SignalRHubs;
using Taskr.MappingProfiles.Job;
using Taskr.Persistance;
using Taskr.Validation.Auth;

namespace Taskr.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             // configuration bindings
            var jwtSettings = new JwtSettings(); 
            Configuration.Bind(nameof(JwtSettings), jwtSettings); 
            services.AddSingleton(jwtSettings);
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            
            
            services.AddControllers().AddFluentValidation(options =>
            {
                options.RegisterValidatorsFromAssemblyContaining<SignUpValidator>();
            }).AddNewtonsoftJson(); 
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
                    builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:3000").AllowCredentials();
                });
            });
            
            // SignalR
            services.AddSignalR();
            
            // AutoMapper
            services.AddAutoMapper(typeof(JobProfile).Assembly);
            
            // EntityFramework
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DatabaseConnectionString"), o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery));
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
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notif"))
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Taskr_Api v1"));
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();
            
            app.UseAuthentication();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<PortalHub>("/notif");

            });
        }
    }
}