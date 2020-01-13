using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Routing;
using RTL.TvMaze.Infrastructure.Services;
using RTL.TvMaze.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RTL.TvMaze.Infrastructure.Entities;
using RTL.TvMaze.Domain.TvMaze.Queries;
using RTL.TvMaze.Domain.TvMaze.Comparers;
using RTL.TvMaze.Infrastructure.Configurations;
using RTL.TvMaze.Api.Scraper.Configurations;

namespace RTL.TvMaze.Api.Scraper
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
            var infrastructureAssembly = AppDomain.CurrentDomain.Load("RTL.TvMaze.Domain");
            services.AddMediatR(infrastructureAssembly);

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.Configure<TvMazeApiSettings>(Configuration.GetSection("TvMazeApiSettings"));
            services.Configure<DownloadSettings>(Configuration.GetSection("DownloadSettings"));

            services.AddDbContext<RTLDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TvMazeDb")));

            services.AddAutoMapper(config => {
                config.AddProfile<DownloadShowIndexProfile>();
                config.AddProfile<GetLatestShowIndexScanProfile>();
            }, typeof(DownloadShowIndexProfile).Assembly);

            services.AddTransient<IHttpTvMazeService, HttpTvMazeApiService>();
            services.AddTransient<ITvMazeShowRepository, TvMazeShowRepository>();
            services.AddTransient<ITvMazeShowIndexRepository, TvMazeShowIndexRepository>();
            services.AddTransient<ITvMazeShowCastRepository, TvMazeShowCastRepository>();
            services.AddTransient<ITvMazePersonRepository, TvMazePersonRepository>();
            services.AddTransient<ITvMazeCastModelEqualityComparer, TvMazeCastModelEqualityComparer>();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
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

            app.UseCors();

            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}
