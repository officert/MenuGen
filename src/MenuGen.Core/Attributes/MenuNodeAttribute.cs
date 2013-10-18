using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace MenuGen.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class MenuNodeAttribute : Attribute
    {
        /// <summary>
        /// The unique identifier for this node.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// The key of a this node's parent node.
        /// </summary>
        public string ParentKey { get; set; }

        /// <summary>
        /// The text that will be used for the anchor text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// The order of this node.
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Whether or not this node is clickable.
        /// </summary>
        public bool Clickable { get; set; }

        /// <summary>
        /// An array of menu types this node belongs too. Invalid menu types will cause an exception to be thrown when the menus are generated.
        /// </summary>
        public Type[] Menus { get; set; }

        /// <summary>
        /// Additional routing data for the node.
        /// </summary>
        public RouteValueDictionary RouteValues { get; set; }

        /// <summary>
        /// Html attributes for the node.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }
}
