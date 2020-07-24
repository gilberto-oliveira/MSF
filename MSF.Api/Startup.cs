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
using MSF.Service.Product;
using MSF.Service.Provider;
using MSF.Service.Shop;
using MSF.Service.State;
using MSF.Service.Stock;
using MSF.Service.WorkCenter;
using System;
using System.Threading.Tasks;

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

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<MSFIdentityDbContext>()
                .AddDefaultTokenProviders();

            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IUserService, UserService>();

            services.AddTransient<ICategoryService, CategoryService>();

            services.AddTransient<IProviderService, ProviderService>();

            services.AddTransient<IStateService, StateService>();

            services.AddTransient<IShopService, ShopService>();

            services.AddTransient<IWorkCenterService, WorkCenterService>();

            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<IStockService, StockService>();

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
                    IssuerSigningKey = signingConfig.Key,
                    ValidAudience = jwtConfig.Audience,
                    ValidIssuer = jwtConfig.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException) || context.Exception.GetType() == typeof(SecurityTokenInvalidSignatureException))
                        {
                            context.Response.Headers.Add("AccessToken-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(auth => {
                auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });

            services.AddCors();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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

            app.UseMvc();
        }
    }
}
