using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.AspNetCore.Routing;
using RTL.TvMaze.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using System;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RTL.TvMaze.Infrastructure.Entities;
using RTL.TvMaze.Domain.TvMaze.Queries;
using RTL.TvMaze.Api.Shows.Mapping;

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

            services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

            services.AddDbContext<RTLDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("TvMazeDb")));

            services.AddAutoMapper(config =>
            {
                config.AddProfile<GetShowAndCastProfile>();
                config.AddProfile<ShowProfile>();
            }, typeof(GetShowAndCastProfile).Assembly, typeof(ShowProfile).Assembly);

            services.AddTransient<ITvMazeShowRepository, TvMazeShowRepository>();
            services.AddTransient<ITvMazeShowIndexRepository, TvMazeShowIndexRepository>();
            services.AddTransient<ITvMazeShowCastRepository, TvMazeShowCastRepository>();
            services.AddTransient<ITvMazePersonRepository, TvMazePersonRepository>();

            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
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
