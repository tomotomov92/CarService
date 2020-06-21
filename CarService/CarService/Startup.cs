using BusinessLogic;
using BusinessLogic.BLs;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace CarService
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

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews();
            services.AddTransient<AppDb>(_ => new AppDb(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddTransient<IBaseBL<CarBrandDTO>, CarBrandBL>();
            services.AddTransient<IBaseBL<ClientDTO>, ClientBL>();
            services.AddTransient<IClientCarBL<ClientCarDTO>, ClientCarBL>();
            services.AddTransient<IBaseBL<EmployeeDTO>, EmployeeBL>();
            services.AddTransient<IBaseBL<EmployeeRoleDTO>, EmployeeRoleBL>();
            services.AddTransient<IInspectionBL<InspectionDTO>, InspectionBL>();
            services.AddTransient<IInvoiceBL<InvoiceDTO>, InvoiceBL>();
            services.AddTransient<IBaseBL<ScheduleDTO>, ScheduleBL>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
