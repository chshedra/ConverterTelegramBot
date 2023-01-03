using System;
using ConverterTelegramBot.Commands;
using ConverterTelegramBot.Models;
using ConverterTelegramBot.Services;
using FileHandler;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConverterTelegramBot
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
            services.AddControllers();
            services.AddDbContext<BotDbContext>(
                opt => opt.UseNpgsql(Configuration.GetConnectionString("BotConnection")),
                ServiceLifetime.Singleton
            );

            services.AddSingleton<Bot>();
            services.AddSingleton<ICommandExecutor, CommandExecutor>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ICommand, StartCommand>();
            services.AddSingleton<ICommand, GetTextCommand>();
            services.AddSingleton<ICommand, ConvertPdfCommand>();
            services.AddSingleton<IChatDataProvider, ChatDataProvider>();
            services.AddSingleton<IPdfConverter, PdfConverter>();
        }

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IServiceProvider serviceProvider
        )
        {
            using (
                var serviceScope = app.ApplicationServices
                    .GetService<IServiceScopeFactory>()
                    .CreateScope()
            )
            {
                var context = serviceScope.ServiceProvider.GetRequiredService<BotDbContext>();
                context.Database.EnsureCreated();
            }
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            serviceProvider.GetRequiredService<Bot>().GetBot().Wait();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
