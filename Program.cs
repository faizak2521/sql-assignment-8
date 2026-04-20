
using Microsoft.EntityFrameworkCore;

namespace MyClass.Tables;

public class Tables
{
    public int Id { get; set; }
    public string ItemNum { get; set; } = "";
    public string Description { get; set; } = "";
    public int OnHand { get; set; }
    public string Category { get; set; } = "";
    public int Storehouse { get; set; }
    public decimal Price { get; set; }
}

internal class ProductDbContext : DbContext
{
    public DbSet<Tables> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {   // Next Step: Needs docker access
        optionsBuilder.UseSqlServer(@"Server=SERVER_NAME;Database=DATABASE_NAME;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        using (ProductDbContext context = new ProductDbContext())
        {
        }
    }
}