using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using SkillExchange.AccessService.Models;
using SkillExchange.AccessService.Repository;
using SkillExchange.AccessService.Repository.Exchange_Repository;
using SkillExchange.AccessService.Repository.ImageUpload;
using SkillExchange.AccessService.Services;
using SkillExchange.AccessService.Services.ExchangeService;
using SkillExchange.AccessService.Services.ImageService;
using SkillExchange.AccessService.Services.SkillService;
using Swashbuckle.AspNetCore.Swagger;

namespace SkillExchange.AccessService
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
            services.AddCors(option => option.AddPolicy("AllowIt", builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

            services.AddControllers();
            
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<ISkillRepository, SkillRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IExchangeRepository, ExchangeRepository>();
            services.AddScoped<IExchangeService, ExchangeService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<ISkillService, SkillService>();
            services.AddScoped<IPerson_Has_Need_Skill_Service, Person_Has_Need_Skill_Service>();
            services.AddScoped<IPerson_Has_Need_Skill_Repo, Person_Has_Need_Skill_Repo>();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IUserRepository<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddDefaultTokenProviders();
            services.AddScoped<IDbConnectionProvider, DbConnectionProvider>();

            //Disable the default password validation provided by ASP.NET Core Identity
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            });


            //JWT auth config
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = false;

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false,
                    ValidateActor = false,
                    ValidateTokenReplay = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidIssuer = Configuration.GetSection("Token").GetValue<string>("Issuer"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Token").GetValue<string>("Jwt_Secret")))
                };

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseCors(options => options.WithOrigins(Configuration.GetSection("Token").GetValue<string>("Issuer")).AllowAnyMethod());
            app.UseCors("AllowIt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
