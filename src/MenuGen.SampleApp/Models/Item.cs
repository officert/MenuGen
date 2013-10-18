using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MenuGen.SampleApp.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        public string Description { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}