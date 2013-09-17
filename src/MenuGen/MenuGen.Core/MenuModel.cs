using System.Collections.Generic;

namespace MenuGen.Core
{
    public class MenuModel
    {
        public string Name { get; set; }
        public ICollection<MenuNodeModel> MenuNodes { get; set; }
    }
}
