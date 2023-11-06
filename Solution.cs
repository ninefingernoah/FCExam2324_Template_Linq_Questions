
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

class Solution
{
    //In DataFormats -> Dish
    
    public static IQueryable<Dish> Q1(ExamContext db, string name, decimal minPrice, decimal maxPrice)
    {
        //List down all FoodItems containing the given name within the minimum and maximum prices given
        var foodsWithinPrice = db.FoodItems
                                .Where(f => f.Name.Contains(name) && minPrice <= f.Price && f.Price <= maxPrice)
                                .Select(f => new Dish(f.Name, f.Price, f.Unit));
        
        return foodsWithinPrice;  //change this line (it is now only used to avoid compiler error)         
    }
 
    //In DataFormats -> DishAndCategory

    public static IQueryable<DishAndCategory> Q2(ExamContext db, int customerId)
    {
        //List down all FoodItems including the Category ordered by a Customer (CustomerID given as parameter)
        var customersFood = db.Orders
                            .Where(o => o.CustomerID == customerId)
                            .Join(db.FoodItems, 
                            orders => orders.FoodItemID,
                            food => food.ID,
                            (orders, food) => new {
                                food.Name, food.Price, food.Unit, food.CategoryID
                            }).Join(db.Categories,
                            fo => fo.CategoryID,
                            c => c.ID,
                            (fo, c) => new {
                                fo.Name, fo.Price, fo.Unit, cname = c.Name
                            }
                            )
                            .Select(dc => new DishAndCategory(dc.Name, dc.Price, dc.Unit, dc.cname));
        return customersFood;  //change this line (it is now only used to avoid compiler error)  
    }

    //In DataFormats -> CustomerBill (BillItem)
    public static IQueryable<CustomerBill> Q3(ExamContext db, int number)
    {
        //List down the bills: FoodItems, Order Quantity, Unit Price, Total,
        //for the first "number" of Customers (ordered based on Total). 
        //Return an Iqueryable<CustomerBill> which will let fetch exactly the "number" of bills
        var billsDesc = db.Orders
                        .Join(db.FoodItems,
                        o => o.FoodItemID,
                        f => f.ID,
                        (o, f) => new {
                            o.CustomerID, f.Name, total = f.Price * o.Quantity //billitems
                        })
                        .GroupBy(of => new {
                            of.CustomerID,
                            of.Name,
                            of.total
                        });
        
        return default(IQueryable<CustomerBill>); //change this line (it is now only used to avoid compiler error)  
    }

    public static IQueryable<Dish> Q4(ExamContext db, int tableNumber)
    {
        // List down dishes >>>NOT<<< sold at a given table
        // Ordering according to the dish price.
        var nonSoldDishes = db.FoodItems
                            .Join(db.Orders,
                            f => f.ID,
                            o => o.FoodItemID,
                            (f , o) => new {
                                f.Name, f.Price, f.Unit, o.FoodItemID, o.CustomerID
                            })
                            .Join(db.Customers,
                            fo => fo.CustomerID,
                            c => c.ID,
                            (fo, c) => new { fo.Name, fo.Price, fo.Unit, fo.CustomerID, c.TableNumber})
                            .Where(foc => foc.TableNumber != tableNumber)
                            .Select(foc => new Dish(foc.Name, foc.Price, foc.Unit));
        return nonSoldDishes;  //change this line (it is now only used to avoid compiler error)  
    }
  
    //In DataFormats -> record DishWithCategories

    public static IQueryable<DishWithCategories> Q5(ExamContext db)
    {
        /*
        Produce a categorised list contanining Dishes (FoodItems projected into Dishes) belonging to each sub category, 
        HINT: A sub category is the one whose [Main] CategoryID is not null.
        HINT: find first those (sub) categories and the corresponding MainCategory (CategoryID attribute).
        Including (Sub) Category and MainCategory (DishWithCategories objects). Self Join
        HINT: Don't forget to add a category even if there is no food item for the given category. Outer Join
        */
        var catDishes = db.FoodItems
                        .Join(db.Categories,
                        f => f.CategoryID,
                        c=> c.ID,
                        (f,c) => new { f.Name, f.Price, f.Unit, f.CategoryID, scname = c.Name})
                        .Join(db.Categories,
                        fc => fc.CategoryID,
                        c => c.CategoryID,
                        (fc, c) => new {fc.Name, fc.Price, fc.Unit, cname = c.Name, scname = fc.scname})
                        .Select(fcc => new DishWithCategories() {
                            MainCategory = fcc.cname,
                            CategoryName = fcc.scname,
                            Food = new Dish(fcc.Name, fcc.Price, fcc.Unit)
                        });
        return catDishes;  //change this line (it is now only used to avoid compiler error)        
   
    }
    
    public static int Q6(ExamContext db, string firstCategory, string secondCategory) {
        
        //This method returns the number of records changed in the DB after these operations have been applied:
        //Create TWO customers and add them.
        //The Name of the customer, ID, TableNumber, and Datetime are 
        //almost (ID has to be unique) fully optional.
        //Each of the two customers will place TWO orders (per customer) for fooditems belonging to the two categories.
        //So customer one -> order1{customer1, firstCategory product1}; order2{customer1, secondCategory product2}
        //   customer two -> order3{customer2, firstCategory product3}; order4{customer2, secondCategory product4}
        //One or both given categories might NOT exist, in this case make sure an order is not placed, 
        //the two customers should be added anyways. 
        int c = 0;
        Customer c1 = new();
        Customer c2 = new();
        db.Customers.Add(c1);
        db.Customers.Add(c2);
        db.SaveChanges();
        Category first = db.Categories.FirstOrDefault(c=>c.Name == firstCategory);
        if ( first != null) {
            FoodItem food = db.FoodItems.FirstOrDefault(f => f.CategoryID == first.CategoryID);
            if(food != null){
                Order o1 = new()
                {
                    FoodItemID = food.ID,
                    CustomerID = c1.ID,
                    Quantity = 2
                };
                db.Orders.Add(o1);
                c++;
                Order o2 = new()
                {
                    FoodItemID = food.ID,
                    CustomerID = c1.ID,
                    Quantity = 1
                };
                db.Orders.Add(o2);
                c++;
            }
        }
         Category snd = db.Categories.FirstOrDefault(c=>c.Name == secondCategory);
        if ( snd != null) {
            FoodItem food = db.FoodItems.FirstOrDefault(f => f.CategoryID == snd.CategoryID);
            if(food != null){
                Order o1 = new()
                {
                    FoodItemID = food.ID,
                    CustomerID = c1.ID,
                    Quantity = 2
                };
                db.Orders.Add(o1);
                c++;
                Order o2 = new()
                {
                    FoodItemID = food.ID,
                    CustomerID = c1.ID,
                    Quantity = 1
                };
                db.Orders.Add(o2);
                c++;
            }
        }
        return c; //change this line (it is now only used to avoid compiler error)  
    }
}