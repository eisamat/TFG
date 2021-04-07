using System.Drawing;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Server.Database;
using Server.Database.Models;
using Server.Models;
using Server.Services;
using Server.Services.Identity;
using Server.ViewModels;
using Shared.Api.Responses;

namespace Server
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
            services.AddRazorPages(options =>
            {
                options.Conventions.AllowAnonymousToPage("/Index");
            });
            
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("db"));
            });
            
            services.AddAuthentication(TokenAuthenticationDefaults.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, TokenAuthenticationHandler>(TokenAuthenticationDefaults.AuthenticationScheme, _ => { })
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationDefaults.AuthenticationScheme, _ => { });
            
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });  
            
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<ITherapistService, TherapistService>();
            services.AddSingleton<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext appDbContext, ITherapistService _therapistService)
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
            
            appDbContext.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
            
            ConfigureMapper();
        }

        private static void ConfigureMapper()
        {
            TypeAdapterConfig<Patient, PatientViewModel>.NewConfig()
                .Map(dest => dest.Therapist, src => src.Therapist.Id)
                .Map(dest => dest.Name, src => src.FullName);
            
            TypeAdapterConfig<Patient, PatientLoginResponse>.NewConfig()
                .Map(dest => dest.Therapist, src => src.Therapist.Id)
                .Map(dest => dest.Name, src => src.FullName);
        }
    }
}