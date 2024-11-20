using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BrazosApp.Areas.Inspector.Controllers
{
    [Area("Inspector")]
    [Authorize(AuthenticationSchemes = "InspectorLoginScheme", Policy = "InspectorPolicy")]
    public class DashboardController : Controller
    {
        [Route("/Inspector/Dashboard")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
