using BusinessLogic;
using BusinessLogic.BLs;
using BusinessLogic.BLs.Interfaces;
using BusinessLogic.DTOs;
using BusinessLogic.EmailSender;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net;
using System.Net.Mail;

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
                options.IdleTimeout = TimeSpan.FromHours(1D);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddControllersWithViews();
            services.AddTransient<AppDb>(_ => new AppDb(Configuration.GetValue<string>("ConnectionStrings:DefaultConnection")));
            services.AddTransient<SmtpClient>(_ => new SmtpClient
            {
                Host = Configuration.GetValue<string>("SmtpClient:Host"),
                Port = Configuration.GetValue<int>("SmtpClient:Port"),
                EnableSsl = Configuration.GetValue<bool>("SmtpClient:EnableSsl"),
                DeliveryMethod = Configuration.GetValue<SmtpDeliveryMethod>("SmtpClient:DeliveryMethod"),
                UseDefaultCredentials = Configuration.GetValue<bool>("SmtpClient:UseDefaultCredentials"),
                Credentials = new NetworkCredential(Configuration.GetValue<string>("SmtpClient:FromAddress"), Configuration.GetValue<string>("SmtpClient:FromPassword"))
            });
            services.AddTransient<MailAddress>(_ => new MailAddress(Configuration.GetValue<string>("SmtpClient:FromAddress"), Configuration.GetValue<string>("SmtpClient:DisplayName")));

            services.AddTransient<IBaseBL<CarBrandDTO>, CarBrandBL>();
            services.AddTransient<ICredentialBL<ClientDTO>, ClientBL>();
            services.AddTransient<IClientCarBL<CarDTO>, ClientCarBL>();
            services.AddTransient<ICredentialBL<EmployeeDTO>, EmployeeBL>();
            services.AddTransient<IBaseBL<EmployeeRoleDTO>, EmployeeRoleBL>();
            services.AddTransient<IInspectionBL<InspectionDTO>, InspectionBL>();
            services.AddTransient<IInvoiceBL<InvoiceDTO>, InvoiceBL>();
            services.AddTransient<IBaseBL<ScheduleDTO>, ScheduleBL>();
            services.AddTransient<EmailSender>();
            services.AddTransient<EmailBL>();
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
