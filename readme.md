<!-- default file list -->
*Files to look at*:
* [Startup.cs](./CS/AspNetCoreDashboard.ExceptionOnDataLoading/Startup.cs) 
* [CustomDashboardController.cs](./CS/AspNetCoreDashboard.ExceptionOnDataLoading/Controllers/CustomDashboardController.cs)
* [Index.cshtml](./CS/AspNetCoreDashboard.ExceptionOnDataLoading/Views/Home/Index.cshtml)

<!-- default file list end -->

# ASP.NET Core Dashboard - How to specify custom exception text
<!-- run online -->
**[[Run Online]](https://codecentral.devexpress.com/267254341/)**
<!-- run online end -->

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

## Documentation

- [Error Logging in Web Dashboard](https://docs.devexpress.com/Dashboard/400015/web-dashboard/error-logging)

## More Examples

- [ASP.NET Core Dashboard - How to throw a custom exception](https://github.com/DevExpress-Examples/asp-net-core-dashboard-throw-custom-exception)
- [ASP.NET MVC Dashboard - How to specify custom exception text (ASPxWebControl.CallbackError)](https://github.com/DevExpress-Examples/asp-net-mvc-dashboard-change-default-error-text-callback-error)
- [ASP.NET MVC Dashboard - How to specify custom exception text (OnException)](https://github.com/DevExpress-Examples/asp-net-mvc-dashboard-change-default-error-text-onException)
- [ASP.NET MVC Dashboard - How to throw a custom exception (ASPxWebControl.CallbackError)](https://github.com/DevExpress-Examples/asp-net-mvc-dashboard-throw-custom-exception-callback-error)
- [ASP.NET MVC Dashboard - How to throw a custom exception](https://github.com/DevExpress-Examples/asp-net-mvc-dashboard-throw-custom-exception-override-on-exception)
- [ASP.NET Web Forms Dashboard - How to specify custom exception text (ASPxWebControl.CallbackError)](https://github.com/DevExpress-Examples/asp-net-web-forms-dashboard-change-default-error-text-callback-error)
- [ASP.NET Web Forms Dashboard - How to throw a custom exception (ASPxWebControl.CallbackError)](https://github.com/DevExpress-Examples/asp-net-web-forms-dashboard-throw-custom-exception-callback-error)
