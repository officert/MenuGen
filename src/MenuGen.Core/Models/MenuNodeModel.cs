using System.Collections.Generic;

namespace MenuGen.Models
{
    public class MenuNodeModel
    {
        public string Text { get; set; }
        public int Order { get; set; }
        public string AreaName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public bool Clickable { get; set; }

        public ICollection<MenuNodeModel> ChildNodes { get; set; }

        public bool IsActive { get; set; }

        public string Key { get; set; }
        public string ParentKey { get; set; }
    }
}
