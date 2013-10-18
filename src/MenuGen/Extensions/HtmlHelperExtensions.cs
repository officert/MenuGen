using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using MenuGen.Models;

namespace MenuGen.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString GetMenu(this HtmlHelper htmlHelper, Type menuType, string templateName = null)
        {
            if (menuType == null)
                throw new ArgumentException("The argument 'menuType' cannot be null.");

            var menuModel = MenuGen.GetMenu(menuType.Name);

            if (menuModel == null)
                throw new ArgumentException(string.Format("No menu with the name {0} exists.", menuType.Name));

            return new HtmlHelper<MenuModel>(htmlHelper.ViewContext, new ViewPage()).DisplayFor(m => menuModel, templateName);
        }
    }
}
