using GreenPipes;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ReportService.Api.Consumer;
using ReportService.Api.Data;
using ReportService.Api.Repository;
using ReportService.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportService.Api
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
            services.AddDbContext<ReportContext>(options => options.UseNpgsql(Environment.GetEnvironmentVariable("DefaultConnection")));

            services.AddScoped<IReportContext>(provider => provider.GetService<ReportContext>());

            services.AddScoped<IReportRepository, ReportRepository>();

            services.AddScoped<ReportService.Api.Services.IReportService, ReportService.Api.Services.ReportService>();

            services.AddAutoMapper(typeof(Startup));

            services.AddMassTransit(configurator =>
            {
                configurator.AddConsumer<ReportRequestConsumer>();
                configurator.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(config =>
                {
                    config.Host(Environment.GetEnvironmentVariable("RABBITMQ_HOSTNAME"), host =>
                    {
                        host.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME"));
                        host.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD"));
                    });
                    config.ReceiveEndpoint(Environment.GetEnvironmentVariable("RABBITMQ_REPORT_QUEUE"), endp =>
                    {
                        endp.PrefetchCount = 1;
                        endp.UseMessageRetry(r => r.Interval(2, 100));
                        endp.ConfigureConsumer<ReportRequestConsumer>(provider);
                    });
                }));
            });
            services.AddMassTransitHostedService();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ReportService.Api", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ReportContext context)
        {
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ReportService.Api v1"));
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
