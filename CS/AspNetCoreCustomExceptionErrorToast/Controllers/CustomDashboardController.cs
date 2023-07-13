using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardWeb;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreThrowCustomExceptionDashboardErrorToast.Controllers {
    [TypeFilter(typeof(CustomExceptionFilter))]
    public class CustomDashboardController : DashboardController {
        public CustomDashboardController(DashboardConfigurator configurator) : base(configurator) { }
    }
}
