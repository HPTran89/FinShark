using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext()
        {
                
        }
        public ApplicationDBContext(DbContextOptions options )
            :base (options)
        {
                
        }

        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
    }
}
