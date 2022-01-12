﻿using EddieShop.Core.Entities;
using EddieShop.Core.Hubs;
using EddieShop.Core.Hubs.Interfaces;
using EddieShop.Core.Interfaces.Base;
using EddieShop.Core.Interfaces.Repository;
using EddieShop.Core.Interfaces.Services;
using EddieShop.Core.Services;
using EddieShop.Core.Utilities;
using EddieShop.Infrastructure.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EddieShop.Controller.API
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
            services.AddControllers();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                // Access cors gọi api bình thường
                builder.WithOrigins("https://eddieshop.netlify.app")
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .SetIsOriginAllowed((host) => true);

                // Access cors kết nối client 
            }));
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "EddieShop", Version = "v1" });
            });

            // Định dạng Json trả về các controller
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });  
            // Định dạng Json trả về các signalR
            services.AddSignalR()
            .AddJsonProtocol(options => {
                options.PayloadSerializerOptions.PropertyNamingPolicy = null;
            });

            // Repository DI`
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ISignalRHub, SignalRHub>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            // Service DI
            services.AddScoped(typeof(IBaseService<>), typeof(BaseService<>));
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IOrderService, OrderService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBaseRepository<User> userRepository, IBaseRepository<Admin> adminRepository)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "EddieShopAPI V1");
                c.RoutePrefix = string.Empty;
            });
            // Disable on production environment
            if (env.IsDevelopment())
            {
                // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
                // specifying the Swagger JSON endpoint.
                //app.UseSwaggerUI(c =>
                //{
                //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "EddieShopAPI V1");
                //    c.RoutePrefix = string.Empty;
                //});
                app.UseDeveloperExceptionPage(); 
            } else
            {
                app.UseHsts();
            }
            

            //app.UseHttpsRedirection();

            //app.UseAuthorization();
            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SignalRHub>("/hub/signalr");
            });
            app.UseSignalR(routes =>
            {
                routes.MapHub<SignalRHub>("/hub/signalr");
            });


            EasyTimer.SetTimeout(() => {
                List<string> columns = new List<string>();
                columns.Add("SessionID");

                var listUserID = userRepository.GetAllEntities(null).Select(item => item.UserID);
                User user = new User();
                foreach (var userID in listUserID)
                {
                    user.SessionID = Guid.NewGuid();
                    userRepository.UpdateColumns(user, (Guid)userID, columns, null);
                }

                var listAdminID = adminRepository.GetAllEntities(null).Select(item => item.AdminID);
                Admin admin = new Admin();
                foreach (var adminID in listAdminID)
                {
                    admin.SessionID = Guid.NewGuid();
                    adminRepository.UpdateColumns(admin, (Guid)adminID, columns, null);
                }
            }, 1000 * 43200);
        }
    }
}
