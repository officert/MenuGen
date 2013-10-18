using System.Data.Entity;
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

        [MenuNode(ParentKey = "Home", Key = "Store", Text = "Store", Menus = new[] { typeof(HeaderMenu) })]
        public ActionResult Index()
        {
            var items = _dbContext.GetDbSet<Item>().Include(x => x.Categories);

            return View(items);
        }

        public ActionResult Item(int id)
        {
            return View();
        }
    }
}
