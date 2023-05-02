using Microsoft.EntityFrameworkCore;


namespace TazaShop.Models
{
    public partial class ShopDbContext:DbContext
    {
        IConfiguration _config;
        public ShopDbContext() { }

        public ShopDbContext(DbContextOptions<ShopDbContext> options, IConfiguration config)
            : base(options) { 
            _config = config;
        }

        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Kind> Kinds { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Cart> Carts { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=tazashop;Trusted_Connection=True;");
                //optionsBuilder.UseSqlServer(_config["ShopDb"]);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
