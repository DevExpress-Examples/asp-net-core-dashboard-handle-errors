using DevExpress.AspNetCore;
using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.ConnectionParameters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace AspNetCoreCustomTextForInternalDashboardErrors {
    public class Startup {

        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment) {
            FileProvider = hostingEnvironment.ContentRootFileProvider;
        }

        public IFileProvider FileProvider { get; }

        public void ConfigureServices(IServiceCollection services) {
            services
                .AddMvc(options => {
                    // Uncomment this line to catch all exceptions (not only from Dashboard):
                    //options.Filters.Add(typeof(CustomExceptionFilter)); 
                })
                .AddDefaultDashboardController(configurator => {
                    DashboardFileStorage dashboardStorage = new DashboardFileStorage(FileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath);
                    DataSourceInMemoryStorage dataSourceStrorage = new DataSourceInMemoryStorage();

                    configurator.SetDashboardStorage(dashboardStorage);
                    configurator.SetDataSourceStorage(dataSourceStrorage);
                    configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;

                });

            services.AddDevExpressControls();
        }

        void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e) {
            // Invalid connection parameters:
            switch (e.DataSourceName) {
                case "sql":
                    e.ConnectionParameters = new MsSqlConnectionParameters(@"localhost", "Northwind123", null, null, MsSqlAuthorizationType.Windows);
                    break;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseDevExpressControls();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                EndpointRouteBuilderExtension.MapDashboardRoute(endpoints, "api/dashboards", "CustomDashboard");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
            });

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

            context.Result = new ContentResult {
                Content = GetJson(!isDevelopmentMode ? "Custom exception text for end users" : "Custom exception text for developers"),
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.BadRequest
            };            

            context.ExceptionHandled = true;
        }
    }
}
