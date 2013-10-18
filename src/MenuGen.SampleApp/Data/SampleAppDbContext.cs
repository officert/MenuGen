using System.Data.Entity;
using MenuGen.SampleApp.Models;

namespace MenuGen.SampleApp.Data
{
    public class SampleAppDbContext : DbContext, IDbContext
    {
        public IDbSet<Item> Items { get; set; }
        public IDbSet<Category> Categories { get; set; } 

        public IDbSet<T> GetDbSet<T>() where T : class
        {
            return Set<T>();
        }
    }
}