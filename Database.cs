using System;
using System.Collections.Generic;

class Database : IDatabase
{
    private List<User> users;
    private List<Product> products;
    private List<Purchase> purchases;

    public Database()
    {
        users = new List<User>
        {
            new User { Username = "John", Balance = 1000.0 },
            new User { Username = "Anna", Balance = 800.0 }
        };

        products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 800.0, Stock = 5 },
            new Product { Id = 2, Name = "Phone", Price = 500.0, Stock = 4 },
            new Product { Id = 3, Name = "Headphones", Price = 100.0, Stock = 9 },
            new Product { Id = 4, Name = "Monitor", Price = 300.0, Stock = 8 },
            new Product { Id = 5, Name = "Keyboard", Price = 50.0, Stock = 6 },
            new Product { Id = 6, Name = "Mouse", Price = 30.0, Stock = 7 },
            new Product { Id = 7, Name = "Tablet", Price = 200.0, Stock = 3 },
            new Product { Id = 8, Name = "Printer", Price = 150.0, Stock = 2 },
            new Product { Id = 9, Name = "Smartwatch", Price = 120.0, Stock = 5 }
        };
        
        purchases = new List<Purchase>
        {
            new Purchase { User = users[0], Product = products[0], Quantity = 2,},
            new Purchase { User = users[1], Product = products[3], Quantity = 1, },
            new Purchase { User = users[0], Product = products[2], Quantity = 3, }
        };
    }

    public void AddUser(User user)
    {
        users.Add(user);
    }

    public User GetUser(string username)
    {
        return users.Find(u => u.Username == username);
    }

    public void AddPurchase(User user, Product product, int quantity)
    {
        if (quantity > product.Stock)
        {
            Console.WriteLine("Invalid input. Please check the product ID and quantity.");
            return;
        }

        double totalCost = product.Price * quantity;

        if (user.Balance < totalCost)
        {
            Console.WriteLine($"Insufficient funds. You have ${user.Balance}, but the total cost is ${totalCost}.");
            return;
        }

        user.Balance -= totalCost;
        product.Stock -= quantity;

        purchases.Add(new Purchase { User = user, Product = product, Quantity = quantity, });

        Console.WriteLine($"{user.Username} purchased {quantity} {product.Name}(s) for ${totalCost}.");
    }

    public List<Purchase> GetPurchaseHistory(User user)
    {
        return purchases.FindAll(p => p.User == user);
    }

    public List<Product> GetProducts()
    {
        return products;
    }

    public void AddProduct(Product product)
    {
        products.Add(product); 
    }

    public Product GetProductById(int productId)
    {
        return products.Find(p => p.Id == productId);
    }

    public List<User> GetUserList()
    {
        return users;
    }
}