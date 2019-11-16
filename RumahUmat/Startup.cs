using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RumahUmat.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using RumahUmat.Interface;
using RumahUmat.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace RumahUmat
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Env { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddMvc(options => options.EnableEndpointRouting = true)
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();

            services.
                //AddMvc().
                AddMvc(options =>
                {
                    options.RespectBrowserAcceptHeader = true; // false by default
                    //options.InputFormatters.Insert(0, new RawRequestBodyFormatter());
                }).
                SetCompatibilityVersion(CompatibilityVersion.Version_2_2).//;
                AddJsonOptions(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver()).
                AddJsonOptions(options => options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Local);

            BHConfiguration settings = Configuration.GetSection("appSetting").Get<BHConfiguration>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            services.AddSingleton(settings);
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            //services.AddSingleton<IConfiguration>(Configuration);

            services.AddHttpContextAccessor();
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            //app.UseCors(builder =>
            //        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            app.UseCors(builder => builder.AllowAnyHeader()
                                    .AllowAnyOrigin()
                                    .WithMethods("GET", "POST","PUT","DELETE","OPTIONS"));

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    c.RoutePrefix = "RumahUmat";
            //});

            app.UseMvc(routes =>
            {
                routes.MapRoute("DefaultApi", "RumahUmat/{controller}/{action}/{id?}");
                //routes.MapRoute("berkah.api", "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
