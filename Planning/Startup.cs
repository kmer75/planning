using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using PlanningApi.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using PlanningApi.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PlanningApi.Services;
using Repositories.Repositories;
using Repositories.Model;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;
using PlanningApi.App_Start.Filters;
using PlanningApi.Data;
using AutoMapper;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace PlanningApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940


        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            var environment = Configuration["ApplicationSettings:Environment"];
        }


        public void ConfigureServices(IServiceCollection services)
        {

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            #region Dependency Injection


            services.AddDbContext<ApplicationDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("PlanningApi")));

            services.AddIdentity<User, ApplicationRole>(config => //put all the configuration that you want
            {
                config.SignIn.RequireConfirmedEmail = true;
            })
                                                                .AddEntityFrameworkStores<ApplicationDbContext>()
                                                                .AddDefaultTokenProviders();

            services.TryAddTransient<IJwtFactory, JwtFactory>();

            services.TryAddTransient<IHttpContextAccessor, HttpContextAccessor>();




            #region Repositories

            services.TryAddTransient<IUserRepository, UserRepository>();

            #endregion


            #region Services


            services.TryAddTransient<IAuthenticationService, AuthenticationService>();

            services.TryAddTransient<IUserService, UserService>();

            services.TryAddTransient<IRegisterService, RegisterService>();

            services.TryAddTransient<IEmailSender, EmailSender>();



            #endregion





            #endregion


            #region jwt 



            // Get options from app settings
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            //secretKey from appsetting.json
            string SecretKey = jwtAppSettingOptions["SecretKey"];
            SymmetricSecurityKey _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecretKey));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.ExpiryMinutes = Int32.Parse(jwtAppSettingOptions[nameof(JwtIssuerOptions.ExpiryMinutes)]);
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                });

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });



            #endregion



            #region Identity

            var builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            #endregion

            #region Email

            // Configure EmailSettings
            // Get options from app settings
            var emailSettingsOptions = Configuration.GetSection(nameof(EmailSettings));

            services.Configure<EmailSettings>(options =>
            {
                options.Host = emailSettingsOptions[nameof(EmailSettings.Host)];
                options.Port = Int32.Parse(emailSettingsOptions[nameof(EmailSettings.Port)]);
                options.UserName = emailSettingsOptions[nameof(EmailSettings.UserName)];
                options.Password = emailSettingsOptions[nameof(EmailSettings.Password)];
                options.FromEmail = emailSettingsOptions[nameof(EmailSettings.FromEmail)];
                options.EnableSsl = Boolean.Parse(emailSettingsOptions[nameof(EmailSettings.EnableSsl)]);
            });

            #endregion

            #region Authorization access

            // api user claim policy : add all the roles here then use these policies for the controllers
            services.AddAuthorization(options =>
            {
                options.AddPolicy("API_ACCESS", policy => policy.RequireClaim(ClaimTypes.Role, Constants.JwtClaims.ApiAccess));
                options.AddPolicy("ADMIN", policy => policy.RequireClaim(ClaimTypes.Role, "ADMIN"));
                options.AddPolicy("USER", policy => policy.RequireClaim(ClaimTypes.Role, "USER"));
            });

            #endregion


            services.AddMvc(
                option =>
                    {
                        option.Filters.Add(new ModelStateErrorFilter()); // an instance
                    }
                ).AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();
            services.AddCors(); //allows cross origin domain

            #region swagger

            //api documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "API References", Version = "v1" });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                // Swagger 2.+ support
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(security);

            });

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, UserManager<User> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext ctx)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            //Get an instance of IServiceProvider : have to be used ONLY if you can't inject the service that you want for come reasons.
            ServiceProviderFactory.Instance = app.ApplicationServices;

            #region seed
            //init datas for initializing the DB. uncomment this code after migration of the roles.
            DataInitializer.SeedData(userManager, roleManager, ctx);
            #endregion

            app.UseCors(builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetPreflightMaxAge(TimeSpan.FromSeconds(3600))
                );

            app.UseExceptionHandler(
          builder =>
          {
              builder.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.Headers.Add("Access-Control-Allow-Origin", "*");

                            var error = context.Features.Get<IExceptionHandlerFeature>();
                            if (error != null)
                            {
                                context.Response.AddApplicationError(error.Error.Message);
                                await context.Response.WriteAsync(error.Error.Message).ConfigureAwait(false);
                            }
                        });
          });

            app.UseAuthentication();


            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "API V1");
            });

        }

    }
}
