using System.Collections.Generic;

namespace MenuGen.Models
{
    public class MenuModel
    {
        public string Name { get; set; }
        public IEnumerable<MenuNodeModel> MenuNodes { get; set; }
    }
}
