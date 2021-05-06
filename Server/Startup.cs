using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
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
            services.AddScoped<IVideoService, VideoService>();
            services.AddScoped<ITherapistService, TherapistService>();
            services.AddSingleton<ITokenService, TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext appDbContext)
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
            SeedDatabase(appDbContext);
        }

        private static void ConfigureMapper()
        {
            TypeAdapterConfig<Patient, PatientViewModel>.NewConfig()
                .Map(dest => dest.Therapist, src => src.Therapist.Id)
                .Map(dest => dest.Name, src => src.FullName);

            TypeAdapterConfig<Video, VideoDto>.NewConfig()
                .Map(dest => dest.CategoryId, src => src.Category.Id)
                .Map(dest => dest.CategoryName, src => src.Category.Name);

            TypeAdapterConfig<Patient, PatientLoginResponse>.NewConfig()
                .Map(dest => dest.Therapist, src => src.Therapist.Id)
                .Map(dest => dest.Name, src => src.FullName);
            
            TypeAdapterConfig<Patient, PatientDetailsResponse>.NewConfig()
                .Map(dest => dest.Therapist, src => src.Therapist.Id)
                .Map(dest => dest.Name, src => src.FullName);
        }

        private static void SeedDatabase(AppDbContext context)
        {
            using (context)
            {
                context.Database.EnsureCreated();

                if (context.Categories.Any())
                {
                    return;
                }
                
                // Get the json file
                var videoList = File.ReadAllText("videos.json");
                var videoJson = JObject.Parse(videoList);

                var categories = (JArray) videoJson.GetValue("content");
                
                foreach (var jToken in categories)
                {
                    var jsonCategory = (JObject) jToken;
                    var name = (string) jsonCategory.GetValue("name");
                    var videos = (JArray) jsonCategory.GetValue("videos");

                    var category = new Category
                    {
                        Name = name
                    };

                    context.Categories.Add(category);

                    foreach (var jToken1 in videos)
                    {
                        var video = (JObject) jToken1;
                        var vName = (string) video.GetValue("name");
                        var youtubeId = (string) video.GetValue("youtubeId");
                        
                        context.Videos.AddAsync(new Video
                        {
                            Name = vName,
                            YoutubeId = youtubeId,
                            Category = category
                        });
                    }
                }
                
                context.SaveChanges();
            }
        }
    }
}