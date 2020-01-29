using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Routing;
using RTL.TvMaze.Infrastructure.Repositories;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RTL.TvMaze.Infrastructure.Entities;
using RTL.TvMaze.Domain.TvMaze.Queries;
using RTL.TvMaze.Api.Shows.Mapping;
using Microsoft.Extensions.Hosting;
using RTL.TvMaze.Domain.TvMaze.Comparers;
using Microsoft.AspNetCore.HttpOverrides;
using RTL.TvMaze.Infrastructure.HttpClient;
using Polly;
using RTL.TvMaze.Infrastructure.Services;

namespace RTL.TvMaze.Api
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
            var infrastructureAssembly = AppDomain.CurrentDomain.Load("RTL.TvMaze.Domain");
            services.AddMediatR(infrastructureAssembly);

            services.AddControllers();

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddDbContext<RTLDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TvMazeDb")));

            services.AddAutoMapper(config =>
            {
                config.AddProfile<GetShowAndCastProfile>();
                config.AddProfile<ShowProfile>();
            }, typeof(GetShowAndCastProfile).Assembly, typeof(ShowProfile).Assembly);

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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Test API V1");
            });
        }
    }
}
