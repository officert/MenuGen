using System.Web.Mvc;
using MenuGen.Core.Attributes;
using MenuGen.SampleApp.Menus;

namespace MenuGen.SampleApp.Controllers
{
    public class HomeController : Controller
    {
        [MenuNodeAttribute(Key = "Home", Text = "Home", Menus = new[] { typeof(HeaderNav), typeof(SidebarNav) })]
        public ActionResult Index()
        {
            return View();
        }

        [MenuNode(Key = "About", Text = "About", Menus = new[] { typeof(HeaderNav), typeof(SidebarNav) })]
        public ActionResult About()
        {
            return View();
        }

        [MenuNodeAttribute(ParentKey = "About", Key = "Blog", Text = "Blog", Menus = new[] { typeof(HeaderNav) })]
        public ActionResult Blog()
        {
            return View();
        }
    }
}
