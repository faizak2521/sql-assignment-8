using System.ComponentModel;
using System.Configuration.Assemblies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

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
            AddProducts(context);

            // Adding new records
            while(true)
            {
                Console.WriteLine("\nMenu");
                Console.WriteLine("S. Show the Records");
                Console.WriteLine("A. Add New Record");
                Console.WriteLine("U. Update a Record");
                Console.WriteLine("D. Delete a Record");
                Console.WriteLine("R. Remove All Records");
                Console.WriteLine("Q. Quit");

                // capital letter
                string? choice = Console.ReadLine()?.ToUpper();

                // switch case for optionsBuilder
                switch (choice)
                {
                    case "S":
                        ShowProducts(context);
                        break;

                    case "A":
                        AddNewProduct(context);
                        break;

                    case "U":
                        UpdateProducts(context);
                        break;

                    case "D":
                        DeleteProducts(context);
                        break;

                    case "R":
                        RemoveProducts(context);
                        break;

                    case "Q":
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;

                }

            }

        }
    }

    // Adding table records
    static void AddProducts(ProductDbContext context)
    {
        if (context.Products.Any()) return; // prevents duplicates

        var products = new List<Product>
        {
            new Product { ItemNum="AH74", Description="Patience", OnHand=9, Category="GME", Storehouse=3, Price=22.99M },
            new Product { ItemNum="BR23", Description="Skittles", OnHand=21, Category="GME", Storehouse=2, Price=29.99M },
            new Product { ItemNum="CD33", Description="Wood Block Set (48 piece)", OnHand=36, Category="TOY", Storehouse=1, Price=89.49M },
            new Product { ItemNum="DL51", Description="Classic Railway Set", OnHand=12, Category="TOY", Storehouse=3, Price=107.95M },
            new Product { ItemNum="DR67", Description="Giant Star Brain Teaser", OnHand=24, Category="PZL", Storehouse=2, Price=31.95M },
            new Product { ItemNum="DW23", Description="Mancala", OnHand=40, Category="GME", Storehouse=3, Price=50.00M },
            new Product { ItemNum="FD11", Description="Rocking Horse", OnHand=8, Category="TOY", Storehouse=3, Price=124.95M },
            new Product { ItemNum="FH24", Description="Puzzle Gift Set", OnHand=65, Category="PZL", Storehouse=1, Price=38.95M },
            new Product { ItemNum="KA12", Description="Cribbage Set", OnHand=56, Category="GME", Storehouse=3, Price=75.00M },
            new Product { ItemNum="KD34", Description="Pentominoes Brain Teaser", OnHand=60, Category="PZL", Storehouse=2, Price=14.95M },
            new Product { ItemNum="KL78", Description="Pick Up Sticks", OnHand=110, Category="GME", Storehouse=1, Price=10.95M },
            new Product { ItemNum="MT03", Description="Zauberkasten Brain Teaser", OnHand=45, Category="PZL", Storehouse=1, Price=45.79M },
            new Product { ItemNum="NL89", Description="Wood Block Set (62 piece)", OnHand=32, Category="TOY", Storehouse=3, Price=119.75M },
            new Product { ItemNum="TR40", Description="Tic Tac Toe", OnHand=75, Category="GME", Storehouse=2, Price=13.99M },
            new Product { ItemNum="TW35", Description="Fire Engine", OnHand=30, Category="TOY", Storehouse=2, Price=118.95M }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }

    // Adding a new record
    static void AddNewProduct(ProductDbContext context)
    {
        Console.Write("Enter ItemNum: ");
        string? itemNumInput = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(itemNumInput))
        {
            Console.WriteLine("Invalid ItemNum.");
            return;
        }

        string itemNum = itemNumInput.Trim().ToUpper();

        if (context.Products.Any(p => p.ItemNum == itemNum))
        {
            Console.WriteLine("Item already exists.");
            return;
        }

        Console.Write("Enter Description: ");
        string? descriptionInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(descriptionInput))
        {
            Console.WriteLine("Invalid Description.");
            return;
        }
        string description = descriptionInput.Trim();

        Console.Write("Enter OnHand: ");
        string? onHandInput = Console.ReadLine();
        if (!int.TryParse(onHandInput, out int onHand))
        {
            Console.WriteLine("Invalid OnHand.");
            return;
        }

        Console.Write("Enter Category: ");
        string? categoryInput = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(categoryInput))
        {
            Console.WriteLine("Invalid Category.");
            return;
        }
        string category = categoryInput.Trim().ToUpper();

        Console.Write("Enter Storehouse: ");
        string? storehouseInput = Console.ReadLine();
        if (!int.TryParse(storehouseInput, out int storehouse))
        {
            Console.WriteLine("Invalid Storehouse.");
            return;
        }

        Console.Write("Enter Price: ");
        string? priceInput = Console.ReadLine();
        if (!decimal.TryParse(priceInput, out decimal price))
        {
            Console.WriteLine("Invalid Price.");
            return;
        }

        Console.WriteLine("New record:");
        Console.WriteLine($"Id: 0");
        Console.WriteLine($"ItemNum: {itemNum}");
        Console.WriteLine($"Description: {description}");
        Console.WriteLine($"OnHand: {onHand}");
        Console.WriteLine($"Category: {category}");
        Console.WriteLine($"Storehouse: {storehouse}");
        Console.WriteLine($"Price: {price:C}");

        Console.Write("Confirm add? (Y/N): ");
        string? confirm = Console.ReadLine()?.ToUpper();

        if (confirm != "Y")
        {
            Console.WriteLine("Add cancelled.");
            return;
        }

        Product product = new Product
        {
            Id = 0,
            ItemNum = itemNum,
            Description = description,
            OnHand = onHand,
            Category = category,
            Storehouse = storehouse,
            Price = price
        };

        context.Products.Add(product);
        context.SaveChanges();
        Console.WriteLine("Record added successfully.");
    }

    // Shows records
    static void ShowProducts(ProductDbContext context)
    {
        var products = context.Products.ToList();

        foreach (var p in products)
        {
            Console.WriteLine($"{p.ItemNum} | {p.Description} | {p.OnHand} | {p.Category} | {p.Storehouse} | {p.Price:C}");
        }
    }

    // Updating records
    static void UpdateProducts(ProductDbContext context)
    {
        Console.Write("Needed Item: "); 
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid ItemNum.");
            return;
        }

        string itemNum = input;
        
        var product = context.Products.FirstOrDefault(p => p.ItemNum == itemNum);
        if (product == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }
        Console.WriteLine("Item found.");

        // Showing submenu
        Console.WriteLine("D. Description");
        Console.WriteLine("O. OnHand");
        Console.WriteLine("C. Category");
        Console.WriteLine("S. Storehouse");
        Console.WriteLine("P. Price");
        Console.WriteLine("E. Exit");

        // Read choice for switch menu
        Console.Write("Choice to update: ");
        string? choice = Console.ReadLine()?.ToUpper();

        // Submenu logic
        switch(choice)
        {
            case "D":
                Console.Write("New Description: ");
                string? newDescription = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newDescription))
                {
                    Console.WriteLine("Invalid Description");
                    return;
                }

                product.Description = newDescription;
                break;

            case "O":
                Console.Write("New OnHand: ");
                string? onHandInput = Console.ReadLine();

                if (!int.TryParse(onHandInput, out int newOnHand))
                {
                    Console.WriteLine("Invalid OnHand");
                    return;
                }

                product.OnHand = newOnHand;
                break;

            case "C":
                Console.Write("New Category: ");
                string? newCategory = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(newCategory))
                {
                    Console.WriteLine("Invalid Category");
                    return;
                }

                product.Category = newCategory;
                break;

            case "S":
                Console.Write("New Storehouse: ");
                string? storehouseInput = Console.ReadLine();

                if (!int.TryParse(storehouseInput, out int newStorehouse))
                {
                    Console.WriteLine("Invalid Storehouse");
                    return;
                }

                product.Storehouse = newStorehouse;
                break;

            case "P":
                Console.Write("New Price: ");
                string? priceInput = Console.ReadLine();

                if (!decimal.TryParse(priceInput, out decimal newPrice))
                {
                    Console.WriteLine("Invalid Price");
                    return;
                }

                product.Price = newPrice;
                break;

            case "E":
                Console.WriteLine("Update cancelled.");
                return;

            default:
                Console.WriteLine("Invalid option.");
                return;
        }

        // Saving chages from swithc 
        context.SaveChanges();
        Console.WriteLine("Record updates saved.");

    }

    // Delete Records
    static void DeleteProducts(ProductDbContext context)
    {
        Console.Write("Enter ItemNum to delete: ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("Invalid ItemNum.");
            return;
        }

        string itemNum = input;

        Product? product = context.Products.FirstOrDefault(p => p.ItemNum == itemNum);

        if (product == null)
        {
            Console.WriteLine("Item not found.");
            return;
        }

        Console.WriteLine("Record found:");
        Console.WriteLine($"Id: {product.Id}");
        Console.WriteLine($"ItemNum: {product.ItemNum}");
        Console.WriteLine($"Description: {product.Description}");
        Console.WriteLine($"OnHand: {product.OnHand}");
        Console.WriteLine($"Category: {product.Category}");
        Console.WriteLine($"Storehouse: {product.Storehouse}");
        Console.WriteLine($"Price: {product.Price:C}");

        Console.Write("Confirm delete? (Y/N): ");
        string? confirm = Console.ReadLine()?.ToUpper();

        if (confirm != "Y")
        {
            Console.WriteLine("Delete cancelled.");
            return;
        }

        context.Products.Remove(product);
        context.SaveChanges();
        Console.WriteLine("Record deleted successfully.");
    }
    
    // Removing all records
    static void RemoveProducts(ProductDbContext context)
    {
        var products = context.Products.ToList();

        if (!products.Any())
        {
            Console.WriteLine("No records to remove.");
            return;
        }

        Console.WriteLine("All records:");
        foreach (var p in products)
        {
            Console.WriteLine($"{p.ItemNum} | {p.Description} | {p.OnHand} | {p.Category} | {p.Storehouse} | {p.Price:C}");
        }

        Console.Write("Confirm remove all records? (Y/N): ");
        string? confirm = Console.ReadLine()?.ToUpper();

        if (confirm != "Y")
        {
            Console.WriteLine("Remove all cancelled.");
            return;
        }

        context.Products.RemoveRange(products);
        context.SaveChanges();
        Console.WriteLine("All records removed successfully.");
    }
    
}
