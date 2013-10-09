using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.SampleApp.Menus;

namespace MenuGen.SampleApp.Controllers
{
    public class EmployeesController : Controller
    {
        [MenuNode(ParentKey = "About", Key = "Employees", Text = "Employees", Menus = new[] { typeof(HeaderMenu) })]
        public ActionResult Index()
        {
            return View();
        }
    }
}
