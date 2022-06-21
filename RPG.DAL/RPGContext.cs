using Microsoft.EntityFrameworkCore;
using RPG.DAL.Models.Items;

namespace RPG.DAL
{
    public class RPGContext :DbContext
    {
        public RPGContext(DbContextOptions<RPGContext> options) : base (options) { }
        public DbSet<Item> Item { get; set; }
    }
}
