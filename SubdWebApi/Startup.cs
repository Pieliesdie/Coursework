using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using SubdWebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Model;
using Microsoft.Extensions.Configuration;

namespace TokenApp
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }
        public IHostingEnvironment Environment { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Environment = environment;
            Configuration = new ConfigurationBuilder()
                     .AddJsonFile("appsettings.json")
                     .AddJsonFile($"appsettings.{Environment.EnvironmentName}.json")
                     .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // укзывает, будет ли валидироваться издатель при валидации токена
                            ValidateIssuer = true,
                            // строка, представляющая издателя
                            ValidIssuer = AuthOptions.ISSUER,

                            // будет ли валидироваться потребитель токена
                           ValidateAudience = true,
                            // установка потребителя токена
                           ValidAudience = AuthOptions.AUDIENCE,
                            // будет ли валидироваться время существования
                            ValidateLifetime = true,

                            // установка ключа безопасности
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // валидация ключа безопасности
                            ValidateIssuerSigningKey = true,
                        };
                    });
            string con = Configuration["Connection"];

            services.AddDbContext<NotConsultantv2Context>(options => options.UseSqlServer(con));
            services.AddMvc(option => option.EnableEndpointRouting = false);
            services.AddHttpContextAccessor();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
