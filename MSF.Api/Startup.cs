using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MSF.Common.Models;
using MSF.Domain.Context;
using MSF.Domain.UnitOfWork;
using MSF.Identity.Context;
using MSF.Identity.Models;
using MSF.Service.Category;
using MSF.Service.Identity;
using MSF.Service.Operation;
using MSF.Service.Product;
using MSF.Service.Provider;
using MSF.Service.Shop;
using MSF.Service.State;
using MSF.Service.Stock;
using MSF.Service.WorkCenter;
using MSF.Service.WorkCenterControl;
using System;

namespace MSF.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<IMSFDbContext, MSFDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MSF_DEV")));

            services.AddDbContext<MSFIdentityDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MSF_DEV")));

            services.AddIdentity<User, Role>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddEntityFrameworkStores<MSFIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<IRoleService, RoleService>();

            services.AddTransient<ICategoryService, CategoryService>();

            services.AddTransient<IProviderService, ProviderService>();

            services.AddTransient<IStateService, StateService>();

            services.AddTransient<IShopService, ShopService>();

            services.AddTransient<IWorkCenterService, WorkCenterService>();

            services.AddTransient<IWorkCenterControlService, WorkCenterControlService>();

            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IStockService, StockService>();

            services.AddTransient<IOperationService, OperationService>();

            var signingConfig = new SigningConfig();

            services.AddSingleton(signingConfig);

            var jwtConfig = new JwtConfig();
            new ConfigureFromConfigurationOptions<JwtConfig>(
                Configuration.GetSection("JwtConfig")
            ).Configure(jwtConfig);

            services.AddSingleton(jwtConfig);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer,
                    IssuerSigningKey = signingConfig.Key,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddAuthorization(auth => {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.ConfigureProblemDetailsModelState();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().WithExposedHeaders("AccessToken-Expired"));

            app.UseAuthentication();

            app.UseProblemDetailsExceptionHandler();

            app.UseMvc();
        }
    }
}
