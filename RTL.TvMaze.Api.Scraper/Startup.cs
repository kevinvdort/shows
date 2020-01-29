using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Routing;
using RTL.TvMaze.Infrastructure.Services;
using RTL.TvMaze.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RTL.TvMaze.Infrastructure.Entities;
using RTL.TvMaze.Domain.TvMaze.Queries;
using RTL.TvMaze.Domain.TvMaze.Comparers;
using RTL.TvMaze.Infrastructure.Configurations;
using RTL.TvMaze.Api.Scraper.Configurations;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Polly;
using RTL.TvMaze.Infrastructure.HttpClient;

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

            services.AddControllers();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
            services.Configure<TvMazeApiSettings>(Configuration.GetSection("TvMazeApiSettings"));
            services.Configure<DownloadSettings>(Configuration.GetSection("DownloadSettings"));

            services.AddDbContext<RTLDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TvMazeDb")));

            services.AddAutoMapper(config =>
            {
                config.AddProfile<DownloadShowIndexProfile>();
                config.AddProfile<GetLatestShowIndexScanProfile>();
            }, typeof(DownloadShowIndexProfile).Assembly);

            services.AddTransient<IHttpTvMazeApiService, HttpTvMazeApiService>();
            services.AddTransient<ITvMazeShowRepository, TvMazeShowRepository>();
            services.AddTransient<ITvMazeShowIndexRepository, TvMazeShowIndexRepository>();
            services.AddTransient<ITvMazeShowCastRepository, TvMazeShowCastRepository>();
            services.AddTransient<ITvMazePersonRepository, TvMazePersonRepository>();
            services.AddTransient<ITvMazeCastModelEqualityComparer, TvMazeCastModelEqualityComparer>();

            services.AddHttpClient<TvMazeApiHttpClient>()
                    .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromSeconds(10)));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            var webSocketOptions = new WebSocketOptions
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
                ReceiveBufferSize = 4 * 1024
            };
            app.UseWebSockets();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}
