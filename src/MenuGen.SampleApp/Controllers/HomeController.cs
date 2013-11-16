using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.SampleApp.Menus;

namespace MenuGen.SampleApp.Controllers
{
    public class HomeController : Controller
    {
        [MenuNode(Key = "Home", Text = "Welcome", Menus = new[] { typeof(HeaderMenu) })]
        public ActionResult Index()
        {
            return View();
        }

        [MenuNode(Key = "About", Text = "About The Company", Menus = new[] { typeof(HeaderMenu), typeof(SidebarMenu) })]
        public ActionResult About()
        {
            return View();
        }
    }
}
