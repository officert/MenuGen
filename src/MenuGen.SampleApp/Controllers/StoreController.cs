using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using MenuGen.Attributes;
using MenuGen.SampleApp.Data;
using MenuGen.SampleApp.Menus;
using MenuGen.SampleApp.Models;

namespace MenuGen.SampleApp.Controllers
{
    public class StoreController : Controller
    {
        private readonly IDbContext _dbContext;

        public StoreController(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [MenuNode(ParentKey = "Home", Key = "Store", Text = "Store", Menus = new[] { typeof(HeaderMenu), typeof(SidebarMenu) })]
        public ActionResult Index()
        {
            var items = _dbContext.GetDbSet<Item>().Include(x => x.Categories);

            return View(items);
        }

        public ActionResult Item(int id)
        {
            var items = _dbContext.GetDbSet<Item>().Include(x => x.Categories);
            var item = items.FirstOrDefault(x => x.Id == id);
            return View(item);
        }
    }
}
