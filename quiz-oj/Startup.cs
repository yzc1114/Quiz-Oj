using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using quiz_oj.Configs.Middlewares;
using quiz_oj.Controllers;
using quiz_oj.Dao;
using quiz_oj.Dao.impls;
using quiz_oj.Dao.interfaces;
using quiz_oj.DynamicCodeExecutor;
using quiz_oj.Service.impls;
using quiz_oj.Service.interfaces;
using Swashbuckle.AspNetCore.Swagger;

namespace quiz_oj
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
            services.AddDistributedMemoryCache();
            services.AddDbContext<QOJDBContext>(options => options.UseMySql(Configuration.GetValue<string>("QOJConnection")));
            services.AddSwaggerGen();
            services.AddSession(options =>
            {
                options.Cookie.Name= ".Quiz-Oj.Session";
                options.IdleTimeout = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
            });
            services.AddControllers();
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(ResponseFormatFilter));
                options.RespectBrowserAcceptHeader = true;
            });
            services.AddSingleton<SessionUtils, SessionUtils>();
            ConfigureDaos(services);
            ConfigureControllerServices(services);
            ConfigureDynamicCodeExecutor(services);
        }

        private void ConfigureDaos(IServiceCollection services)
        {
            services.AddScoped<IUserInfoDao, UserInfoDao>();
            services.AddScoped<IOjDao, OjDao>();
            services.AddScoped<IQuizDao, QuizDao>();
            services.AddScoped<IReviewDao, ReviewDao>();
            services.AddScoped<DaoUtils, DaoUtils>();
        }

        private void ConfigureControllerServices(IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IOjService, OjService>();
            services.AddScoped<IQuizService, QuizService>();
            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IReviewService, ReviewService>();
        }

        private void ConfigureDynamicCodeExecutor(IServiceCollection services)
        {
            services.AddSingleton<CenterScheduler, CenterScheduler>();
            services.AddSingleton<Compiler, Compiler>();
            services.AddSingleton<Executor, Executor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();
            app.UseSession();
            //app.UseErrorHandling();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
