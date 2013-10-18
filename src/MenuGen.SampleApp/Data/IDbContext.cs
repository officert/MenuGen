
using System.Data.Entity;

namespace MenuGen.SampleApp.Data
{
    public interface IDbContext
    {
        IDbSet<T> GetDbSet<T>() where T : class;
        int SaveChanges();
    }
}