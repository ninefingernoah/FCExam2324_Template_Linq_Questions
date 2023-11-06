
namespace DataFormats {

    //----Question 1-----

    public record Dish
    {    
        public Dish(string name, decimal price, string? unit) {
            Name = name;
            Price = price;
            Unit = unit;
        }
        
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public string? Unit { get; set; } = null!;
    }

    //----Question 2-----

    public record DishAndCategory : Dish
    {        
        public DishAndCategory(string name, decimal price, string? unit, string categoryName) :
                 base(name, price, unit){
            CategoryName = categoryName;
        }
        public string CategoryName { get; set; } = null!;
    }
  
    //----Question 3-----
  
    public record BillItem : Dish
    {       
        public BillItem(string name, decimal price, string? unit, int quantity) : base(name, price, unit) {
            Quantity = quantity;
        }
        public int Quantity { get; set; }
    }

    public record CustomerBill {
        public int CustomerID { get; set; }
        public List<BillItem>? Bill{ get; set; }
        public decimal Total { get; set; }
    }

    //----Question 5-----

    public record DishWithCategories
    {       
        public string MainCategory { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public Dish? Food { get; set; } = null!;
    }

}






