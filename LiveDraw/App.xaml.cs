using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LiveDraw
{

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ILiveDraw
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = WebApplication.CreateBuilder(new string[] { });

            // Add services to the container.

            builder.Services.AddSingleton<ILiveDraw>(this);
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
               .AddNegotiate();

            builder.Services.AddAuthorization(options =>
            {
                // By default, all incoming requests will be authorized according to the default policy.
                options.FallbackPolicy = options.DefaultPolicy;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            var thread = new Thread(() => app.Run());
            thread.Start();            
        }

        private void InvokeMainWindow(Action<AntFu7.LiveDraw.MainWindow> action)
        {
            this.Dispatcher.Invoke(() =>
            {
                var window = (AntFu7.LiveDraw.MainWindow)this.MainWindow;
                action(window);
            });
        }
        
        public string GetSelectedColor()
        {
            string color = string.Empty;
            this.Dispatcher.Invoke(() =>
            {
                var window = (AntFu7.LiveDraw.MainWindow)this.MainWindow;
                color = window.GetSelectedColor().ToString();
            });

            return color;
        }

        public void NextColor()
        {
            this.InvokeMainWindow(w => w.NextColor());
        }

        public void PreviousColor()
        {
            this.InvokeMainWindow(w => w.PreviousColor());
        }
    }
}
