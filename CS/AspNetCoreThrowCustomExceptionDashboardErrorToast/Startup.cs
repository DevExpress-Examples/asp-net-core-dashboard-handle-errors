using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Xml.Linq;
using System.Linq;

namespace AspNetCoreThrowCustomExceptionDashboardErrorToast {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
        }

        public void ConfigureServices(IServiceCollection services) {
            services
				.AddMvc()
				.ConfigureApplicationPartManager((manager) => {
					var dashboardApplicationParts = manager.ApplicationParts.Where(part => 
						part is AssemblyPart && ((AssemblyPart)part).Assembly == typeof(DashboardController).Assembly).ToList();
					foreach(var partToRemove in dashboardApplicationParts) {
					  manager.ApplicationParts.Remove(partToRemove);
					}
				})
                .AddDefaultDashboardController(configurator => {
                    configurator.SetDashboardStorage(new CustomDashboardStorage());
                });

            services.AddDevExpressControls();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints => {
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboards", "CustomDashboard");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

            app.UseStaticFiles();
            app.UseDevExpressControls();
        }
    }

    public class CustomException : Exception {
        public const string SafeMessage = "Custom exception text for end users";
        public const string UnsafeMessage = "Custom exception text for developers";
    }

    public class CustomDashboardStorage : IDashboardStorage {
        IEnumerable<DashboardInfo> IDashboardStorage.GetAvailableDashboardsInfo() {
            return new[] {
                new DashboardInfo { ID = "Dashboard", Name = "Dashboard" }
            };
        }
        XDocument IDashboardStorage.LoadDashboard(string dashboardID) {
            // Custom Exception:
            throw new CustomException();
        }
        void IDashboardStorage.SaveDashboard(string dashboardID, XDocument dashboard) {
        }
    }

    public class CustomExceptionFilter : IExceptionFilter {
        internal bool isDevelopmentMode = false;

        public CustomExceptionFilter(IWebHostEnvironment hostingEnvironment) {
            this.isDevelopmentMode = hostingEnvironment.IsDevelopment();
        }
        string GetJson(string message) {
            return $"{{ \"Message\":\"{message}\" }}";
        }

        public virtual void OnException(ExceptionContext context) {
            if(context.ExceptionHandled || context.Exception == null) {
                return;
            }

            CustomException customException = context.Exception as CustomException;
            string message = customException != null ? (isDevelopmentMode ? CustomException.UnsafeMessage : CustomException.SafeMessage) : "";

            context.Result = new ContentResult {
                Content = GetJson(message),
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.BadRequest
            };

            context.ExceptionHandled = true;
        }
    }
}
