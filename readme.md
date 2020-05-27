# ASP.NET Core Dashboard - How to specify custom exception text

The dashboard in this project contains invalid data connection. This example shows how to override the default text in the exception that occurs when a controller tries to load data.

![](image/web-exception-on-data-loading.png)

Implement the `IExceptionFilter` interface to create a custom exception filter and specify a custom exception message. The displayed text depends on whether the application is in development mode:

```cs
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
```

Create a custom controller that uses the custom exception filter:

```cs
namespace AspNetCoreDashboard.ExceptionOnDataLoading.Controllers
{
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CustomDashboardController : DashboardController {
        public CustomDashboardController(DashboardConfigurator configurator) : base(configurator) { }
    }
}
```

Specify the `CustomDashboard` controller when you configure endpoints:

```cs
public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
	// ...
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
```

