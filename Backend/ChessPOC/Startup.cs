using AutoMapper;
using ChessPOC.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories.GamesRepository;
using Services.ChessVariants;
using Services.GamesManagerService;
using Services.GameTypeDispatcherService;
using Services.GameValidationService;
using Services.MoveProcessorService;
using Services.PiecesManagerService;

namespace ChessPOC
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
            services.AddCors();
            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSignalR(options => options.EnableDetailedErrors = true);

            services.AddSingleton<IGamesRepository, GamesRepository>();
            services.AddScoped<IBaseChessVariantService, ClassicalChessVariantService>();
            services.AddScoped<IBaseChessVariantService, RandomChessVariantService>();
            services.AddScoped<IBaseChessVariantService, KingOfTheHillChessVariantService>();
            services.AddScoped<IGameTypeDispatcherService, GameTypeDispatcherService>();
            services.AddScoped<IPiecesManagerService, PiecesManagerService>();
            services.AddScoped<IGamesManagerService, GamesManagerService>();
            services.AddScoped<IGameValidationService, GameValidationService>();
            services.AddScoped<IMoveProcessorService, MoveProcessorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseCors(builder => builder.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowCredentials().AllowAnyMethod());

            app.UseSignalR(routes => { routes.MapHub<SinglePlayerHub>("/SinglePlayerHub");});

            app.UseMvc();
        }
    }
}
