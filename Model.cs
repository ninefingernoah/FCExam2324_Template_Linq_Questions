
class ExamContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>()
            .HasOne<Category>(_ => _.MainCategory)
            .WithMany()
            .HasForeignKey(_ => _.CategoryID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>().HasKey(_ => new { _.CustomerID, _.FoodItemID });

        modelBuilder.Entity<Order>()
            .HasOne<FoodItem>(_ => _.FoodItem)
            .WithMany()
            .HasForeignKey(_ => _.FoodItemID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>()
            .HasOne<Customer>(_ => _.Customer)
            .WithMany()
            .HasForeignKey(_ => _.CustomerID)
            .OnDelete(DeleteBehavior.Cascade);    
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("User ID = postgres; Password = a; Host = localhost; port = 5432; Database = ExamDB2324; Pooling = true");
        //optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<FoodItem> FoodItems { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
}

class Category
{
    public int ID { get; set; }  //Convention
    public string Name { get; set; } = null!;
    public Category? MainCategory { get; set; }
    public int? CategoryID { get; set; }

    //Constructors
    public Category() { }
    public Category(int id, string name)  { Name = name; ID = id; }
    public Category(int id, string name, int categoryID) : this(id, name) => CategoryID = categoryID;
}

class FoodItem
{
    public int ID { get; set; }  //Convention
    public string Name { get; set; } = null!;
    public Category? category { get; set; }
    public int CategoryID { get; set; }
    public decimal Price { get; set; }
    public string? Unit { get; set; }

    //Constructors
    public FoodItem(int iD, string name, int categoryID, decimal price)
    {
        ID = iD;
        Name = name;
        CategoryID = categoryID;
        Price = price;
    }
    public FoodItem(int iD, string name, int categoryID, decimal price, string unit)
        : this(iD, name, categoryID, price) => Unit = unit;

}

class Customer
{
    public int ID { get; set; }   //Convention
    public string? Name { get; set; }
    public DateTime DateTime { get; set; }
    public int? TableNumber { get; set; }
}

class Order
{
    public Customer Customer { get; set; } = null!;
    public int CustomerID { get; set; }  

    public FoodItem? FoodItem { get; set; }
    public int FoodItemID { get; set; }

    public int Quantity { get; set; }
}

class Seed
{   
    public static void ClearDB(ExamContext db) {
           string truncateTables = @"
                    TRUNCATE TABLE ""Categories"" CASCADE ;
                    TRUNCATE TABLE ""Customers"" CASCADE ;
                    TRUNCATE TABLE ""FoodItems"" CASCADE ;
                    TRUNCATE TABLE ""Orders"" CASCADE ;
                    ";
           db.Database.ExecuteSqlRaw(truncateTables);        
    }

    public static void SeedData(ExamContext db)
    {
        if (db is null)
        {
            Console.WriteLine("ExamContext is null"); return;
        }
        if (db.Orders.Count() != 0)
        {
            return;
            /*
            Console.WriteLine("Clearing DB...");
            ClearDB(db);
            */
        }
        db.Categories.AddRange(GetCategories());
        db.FoodItems.AddRange(GetFoodItems());
        db.Customers.AddRange(GetCustomers());
        db.Orders.AddRange(GetOrders());
        Console.WriteLine($"{db.SaveChanges()} rows effected");
    }

    private static List<Category> GetCategories()
    {
        List<Category> categories = new List<Category>(){
                new Category(){ID = 1, Name="Beverages" },
                new Category(){ID= 2, Name = "Starter" },
                new Category(){ID = 3, Name = "Sea Food" },
                new Category(){ID = 4,
                    Name = "Vegetarian" },
                new Category(){ID = 5,
                    Name = "Meat" },
                new Category(){ID =6,
                    Name = "Dessert" },
                new Category(){ID = 7,
                    Name = "Milk Shake", CategoryID= 1 },
            new Category(){ID= 8, Name ="Hot Drinks",CategoryID = 1 },
            new Category(){ID = 9,Name = "Soft Drinks", CategoryID = 1 },
                new Category(){ID = 10,
                    Name = "Sea Food Starter", CategoryID = 2 },
            new Category(){ID = 11, Name = "Vegetarian Starter",CategoryID =  2}, 
            new Category() {ID = 12, Name = "Meat Starter", CategoryID= 2 }
            };
        return categories;
    }

    private static List<Category> GetCategoriesOld()
    {
        List<Category> categories = new List<Category>(){
                new Category(1, "Beverages"),
                new Category(2, "Starter"),
                new Category(3, "Sea Food"),
                new Category(4, "Vegetarian"),
                new Category(5, "Meat"),
                new Category(6, "Dessert"),
                new Category(7, "Milk Shake", 1), 
                new Category(8, "Hot Drinks", 1), 
                new Category(9,"Soft Drinks", 1),
                new Category(10, "Sea Food Starter", 2), 
                new Category(11, "Vegetarian Starter", 2), 
                new Category(12, "Meat Starter", 2)
            };
        return categories;
    }

    public static List<FoodItem> GetFoodItems()
    {
        List<FoodItem> foodItems = new List<FoodItem>(){
            new FoodItem(1, "Tea", 8, 2.50m, "250ml"),
            new FoodItem(2, "Espresso", 8, 3, "30ml"),
            new FoodItem(3, "Cappuccino", 8, 3.45m, "100ml"),
            new FoodItem(4, "Chinotto", 9, 3.50m, "300ml"),
            new FoodItem(5, "Water", 9, 1.65m ,"1Liter"),
            new FoodItem(6, "Fritto misto", 10, 9.50m, "350g"),
            new FoodItem(7, "Samosa", 11, 5.50m, "100g"),
            new FoodItem(8, "Corn Kebab", 11, 6.50m, "100g"),
            new FoodItem(9, "Chicken Tikka",11, 10.50m, "300g"),
            new FoodItem(10, "Coniglio in agrodolce", 5, 15.55m,"250g"),
            new FoodItem(11, "Spinach", 4, 12m),
            new FoodItem(12, "Aubergine", 4, 9.50m),
            new FoodItem(13, "Broccoli", 4, 8m),
            new FoodItem(14, "Melanzane alla parmigiana", 4, 12.55m),
            new FoodItem(15, "Meatballs", 5, 12m),
            new FoodItem(16, "Chicken Fried Rice",5, 13.50m),
            new FoodItem(17, "Vitello Tonnato", 5, 20m),
            new FoodItem(18, "Chocolate Coffee Truffle", 6, 9m),
            new FoodItem(19, "Apple Pie", 6, 8.50m),
            new FoodItem(20, "Brownies", 6, 9m),
            new FoodItem(21, "Cannoli", 6, 7.25m),
            new FoodItem(22, "Sebadas", 6, 8.75m),
            new FoodItem(23, "Pardula", 6, 3m),
            new FoodItem(24, "Mango Lassi", 7, 2m),
        };

        return foodItems;
    }

    public static List<Customer> GetCustomers()
    {   
        //return File.ReadAllLines(Environment.ProcessPath.Split("bin")[0]+"Customers.csv") //if using VisualStudio use this line
          return File.ReadAllLines("./Customers.csv")  // for VS code, comment it when using VisualStudio
                   .Skip(1) //Header
                   .Where(_ => _.Length > 0)
                   .Select(_ => Customer(_)).ToList();
    }

    private static Customer Customer(string row)
    {
        string format = "dd/MM/yyyy HH:mm";
        CultureInfo provider = CultureInfo.InvariantCulture;
        
        var columns = row.Split(',');
        return new Customer()
        {
            ID = int.Parse(columns[0]),
            Name = columns[1],
            DateTime = DateTime.SpecifyKind(DateTime.ParseExact(columns[2], format, provider), DateTimeKind.Utc),
            TableNumber = int.Parse(columns[3])
        };
    }

    public static List<Order> GetOrders()
    {
        List<Order> orders = new List<Order>();
        Random rand = new Random();
        Order newOrder;
        int maxJ = 0;
        for (int i = 1; i <= 100; i++) {
            maxJ = rand.Next(1,4);
            for (int j = 0; j < maxJ; j++)
            {
                newOrder = new Order()
                {
                    CustomerID = i,
                    FoodItemID = rand.Next(1, 25),
                    Quantity = rand.Next(1, 3)
                };
                if(orders.Count(_ => _.CustomerID == newOrder.CustomerID && _.FoodItemID == newOrder.FoodItemID)==0)
                    orders.Add(newOrder);
            }
        }
        return orders;
    }  
}
