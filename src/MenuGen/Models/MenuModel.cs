using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace MenuGen.Models
{
    public class MenuModel
    {
        public string Name { get; set; }
        public ICollection<MenuNodeModel> MenuNodes { get; set; }

        public MenuModel()
        {
            MenuNodes = new Collection<MenuNodeModel>();
        }
    }
}
