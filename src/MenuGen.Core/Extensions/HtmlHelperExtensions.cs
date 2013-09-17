using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace MenuGen.Core.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString GetMenu(this HtmlHelper htmlHelper, Type menuType, string templateName = null)
        {
            if (menuType == null)
                throw new ArgumentException("The argument 'menuType' cannot be null.");

            if (!menuType.BaseType.IsAssignableFrom(typeof(MenuModel)))
                throw new ArgumentException(string.Format("The Type passed in, '{0}', is not assignable from MapperLite.Core.Menu", menuType.Name));

            var menuModel = MenuGenerator.Menus.FirstOrDefault(x => x.Name == menuType.Name);

            if (menuModel == null)
                throw new ArgumentException(string.Format("No menu with the name {0} exists.", menuType.Name));

            return new HtmlHelper<MenuModel>(htmlHelper.ViewContext, new ViewPage()).DisplayFor(m => menuModel, templateName);
        }
    }
}
