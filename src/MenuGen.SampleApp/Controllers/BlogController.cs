using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.SampleApp.Menus;

namespace MenuGen.SampleApp.Controllers
{
    public class BlogController : Controller
    {
        [MenuNode(ParentKey = "Home", Key = "Blog", Text = "View our Blog", Menus = new[] { typeof(ItemsMenu) })]
        public ActionResult Index()
        {
            return View();
        }

        [MenuNode(ParentKey = "Blog", Key = "Post1", Text = "Post 1 - How does economics apply to healthcare?", Menus = new[] { typeof(ItemsMenu) })]
        public ActionResult Post1()
        {
            return View();
        }

        [MenuNode(ParentKey = "Blog", Key = "Post2", Text = "Post 2 - Why your doctor is afraid of your DNA", Menus = new[] { typeof(ItemsMenu) })]
        public ActionResult Post2()
        {
            return View();
        }

        [MenuNode(ParentKey = "Blog", Key = "Post3", Text = "Post 3 - How herbal medicine works", Menus = new[] { typeof(ItemsMenu) })]
        public ActionResult Post3()
        {
            return View();
        }
    }
}
