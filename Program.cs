using Microsoft.EntityFrameworkCore;

namespace MyClass.Tables;

public class Product
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
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string? password = Environment.GetEnvironmentVariable("DB_PASSWORD");

        if (string.IsNullOrWhiteSpace(password))
        {
            throw new Exception("DB_PASSWORD environment variable is not set.");
        }

        optionsBuilder.UseSqlServer(
            $@"Server=localhost,1433;Database=ProductDB;User Id=sa;Password={password};TrustServerCertificate=True;");
    }
}

internal class Program
{
    static void Main(string[] args)
    {
        using (ProductDbContext context = new ProductDbContext())
        {
            Console.WriteLine("Ready for migrations...");
        }
    }
}