using AutoMapper;
using AutoMapper.Data;
using erifornimento.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApplication3.Db;
using WebApplication3.Domain.Security;
using WebApplication3.Utils;

namespace WebApplication3
{
    public class Startup
    {
        public IHostingEnvironment Env { get; set; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Env = env;

            //var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            //var pathToContentRoot = Path.GetDirectoryName(pathToExe);

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //string connection = @"Server=.\SQLEXPRESS;Database=ERifornimento;Trusted_Connection=True;ConnectRetryCount=0";
            //services.AddDbContext<Db.ApplicationContext>
            //   (options => options.UseSqlServer(Configuration.GetConnectionString("ApplicationDatabase")));
            String connection = Configuration.GetConnectionString("ApplicationDatabase");
            services.AddDbContext<Db.ApplicationContext>
               (options => options.UseSqlServer(connection));

            String iconnection = Configuration.GetConnectionString("IntegrationDatabase");
            services.AddDbContext<Db.IntegrationContext>
               (options => options.UseSqlServer(iconnection));

            services.AddIdentity<User, IdentityRole>(o =>
            {
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 4;
            })
              .AddEntityFrameworkStores<Db.ApplicationContext>()
              .AddDefaultTokenProviders();

            services.AddAutoMapper(c => c.AddDataReaderMapping());
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])),
                        ClockSkew = TimeSpan.Zero// remove delay of token when expire

                    };

                });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddMvc(config =>
            {
                config.Filters.Add(new CustomExceptionFilterAttribute(Env));

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
            .AddJsonOptions(options =>
            {
                options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.IsoDateFormat;
                options.SerializerSettings.Error = (sender, args) =>
                {
                    // Exceptions are being eaten up and ignored right now.  Set breakpoint here in order to debug theproblem.
                    throw new Exception("There was an error during deserialization.");
                };
            });


            services.AddSingleton(Configuration);
            
            services.AddScoped<ArticoliService>();
            services.AddScoped<ClientiService>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ApplicationContext db)
        {
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseHsts();
            }


            app.UseDefaultFiles();
            app.UseStaticFiles();

            //  app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseMvc();


            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationContext>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<User>>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                Task t = DbInitializer.Initialize(context, userManager, roleManager);
                t.Wait();
            }
         

        }
    }
}
