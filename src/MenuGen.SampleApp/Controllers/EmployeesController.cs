using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.SampleApp.Menus;

namespace MenuGen.SampleApp.Controllers
{
    public class EmployeesController : Controller
    {
        [MenuNode(ParentKey = "Home", Key = "Employees", Text = "Employees", Menus = new[] { typeof(HeaderNav) })]
        public ActionResult Index()
        {
            return View();
        }

        [MenuNode(ParentKey = "Employees", Key = "BillHenderson", Text = "Bill Henderson", Menus = new[] { typeof(HeaderNav) })]
        public ActionResult BillHenderson()
        {
            return View();
        }

        [MenuNode(ParentKey = "Employees", Key = "CarrieJefferson", Text = "Carrie Jefferson", Menus = new[] { typeof(HeaderNav) })]
        public ActionResult CarrieJefferson()
        {
            return View();
        }
    }
}
