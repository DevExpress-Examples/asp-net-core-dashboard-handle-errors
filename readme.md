<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/267254341/23.1.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T894095)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->
# BI Dashboard for ASP.NET Core - How to handle errors

The following example demostrates two approaches on how to handle errors in the ASP.NET Core Dashboard application:

- How to specify custom text for internal Dashboard errors
- How to throw a custom exception during a server-side processing and display the error in the Dashboard error toast


## How to specify custom text for internal Dashboard errors

### Files to Review

* [Startup.cs](./CS/AspNetCoreCustomTextForInternalDashboardErrors/Startup.cs) 
* [CustomDashboardController.cs](./CS/AspNetCoreCustomTextForInternalDashboardErrors/Controllers/CustomDashboardController.cs)
* [Index.cshtml](./CS/AspNetCoreCustomTextForInternalDashboardErrors/Views/Home/Index.cshtml)

### Description

The dashboard in this project contains invalid data connection. This example shows how to override the default text in the exception that occurs when a controller tries to load data.

![](image/web-custom-text-for-internal-dashboard-errors.png)

1. Implement the `IExceptionFilter` interface to create a custom exception filter and specify a custom exception message. You can specify the displayed text depending on whether the application is in development mode.
1. Create a custom controller that uses the custom exception filter.
1. Specify the `CustomDashboard` controller when you configure endpoints.

## How to throw a custom exception during a server-side processing and display the error in the Dashboard error toast

### Files to Review

* [Startup.cs](./CS/AspNetCoreCustomExceptionErrorToast/Startup.cs) 
* [CustomDashboardController.cs](./CS/AspNetCoreCustomExceptionErrorToast/Controllers/CustomDashboardController.cs)
* [Index.cshtml](./CS/AspNetCoreCustomExceptionErrorToast/Views/Home/Index.cshtml)

### Description

This example shows how to throw a custom exception when a controller loads a dashboard.

![](image/web-throw-custom-exception-dashboard-toast.png)

1. Implement the `IExceptionFilter` interface to create a custom exception filter. You can specify the displayed text depending on whether the application is in development mode.
1. Create a custom controller that uses the custom exception filter.
1. Specify the `CustomDashboard` controller when you configure endpoints.
1. To throw an exception when the control loads a dashboard, create custom dashboard storage and override the `LoadDashboard` method.

## Documentation

- [Handle and Log Server-Side Errors in ASP.NET Core](https://docs.devexpress.com/Dashboard/400026/web-dashboard/integrate-dashboard-component/aspnet-core-dashboard-control/handle-and-log-server-side-errors-in-asp-net-core)

## More Examples

- [ASP.NET MVC Dashboard - How to handle errors](https://github.com/DevExpress-Examples/asp-net-mvc-dashboard-change-default-error-text-onException)
- [ASP.NET Web Forms Dashboard - How to handle errors](https://github.com/DevExpress-Examples/asp-net-web-forms-dashboard-change-default-error-text-callback-error)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-core-dashboard-handle-errors&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-core-dashboard-handle-errors&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->
