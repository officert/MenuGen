using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;

namespace MenuGen.SampleApp.Models
{
    [TableName("Categories")]
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}