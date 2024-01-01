using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        IDatabase database = new Database();

        int choice;

        do
        {
            DisplayMenu();

            Console.Write("Enter your choice: ");
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        DisplayAvailableProducts(database);
                        break;
                    case 2:
                        AddProduct(database);
                        break;
                    case 3:
                        DisplayProductInfoById(database);
                        break;
                    case 4:
                        DisplayAllUsers(database);
                        break;
                    case 5:
                        MakePurchase(database);
                        break;
                    case 6:
                        RechargeBalance(database);
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }

        } while (choice != 0);
    }

    static void DisplayMenu()
    {
        Console.WriteLine("Main Menu:");
        Console.WriteLine("1. Display Available Products");
        Console.WriteLine("2. Add Product");
        Console.WriteLine("3. Display Product Info by ID");
        Console.WriteLine("4. Display All Users");
        Console.WriteLine("5. Make Purchase");
        Console.WriteLine("6. Recharge Balance");
        Console.WriteLine("0. Exit");
    }

    static void DisplayAvailableProducts(IDatabase database)
    {
        List<Product> availableProducts = database.GetProducts();
        if (availableProducts.Count == 0)
        {
            Console.WriteLine("No available products.");
        }
        else
        {
            Console.WriteLine("Available products:");
            foreach (var product in availableProducts)
            {
                Console.WriteLine($"ID: {product.Id}, {product.Name} (${product.Price}), Stock: {product.Stock}");
            }
        }
    }

    static void AddProduct(IDatabase database)
    {
        Console.Write("Enter product name: ");
        string productName = Console.ReadLine();

        Console.Write("Enter product price: ");
        double productPrice;
        bool isPriceValid = double.TryParse(Console.ReadLine(), out productPrice);

        Console.Write("Enter product stock: ");
        int productStock;
        bool isStockValid = int.TryParse(Console.ReadLine(), out productStock);

        if (isPriceValid && isStockValid)
        {
            Product newProduct = new Product
            {
                Id = database.GetProducts().Count + 1,
                Name = productName,
                Price = productPrice,
                Stock = productStock
            };

            database.AddProduct(newProduct);

            Console.WriteLine($"Product '{productName}' added successfully.");
        }
    }

    static void DisplayProductInfoById(IDatabase database)
    {
        Console.Write("Enter product ID: ");
        int productId;
        if (int.TryParse(Console.ReadLine(), out productId))
        {
            Product product = database.GetProductById(productId);
            if (product != null)
            {
                Console.WriteLine($"Product ID: {product.Id}");
                Console.WriteLine($"Name: {product.Name}");
                Console.WriteLine($"Price: ${product.Price}");
                Console.WriteLine($"Stock: {product.Stock}");
            }
            else
            {
                Console.WriteLine("Invalid product ID. Can't find the product.");
            }
        }
    }
    static void DisplayAllUsers(IDatabase database)
    {
        List<User> userList = database.GetUserList();
        if (userList.Count == 0)
        {
            Console.WriteLine("No users found.");
        }
        else
        {
            Console.WriteLine("User List:");
            foreach (var user in userList)
            {
                Console.WriteLine($"Username: {user.Username}, Balance: ${user.Balance}");

                List<Purchase> purchaseHistory = database.GetPurchaseHistory(user);
                if (purchaseHistory.Count > 0)
                {
                    Console.WriteLine("Purchase History:");
                    foreach (var purchase in purchaseHistory)
                    {
                        Console.WriteLine($"Product: {purchase.Product.Name}, Quantity: {purchase.Quantity}, Date: {purchase.PurchaseDate}");
                    }
                }
                else
                {
                    Console.WriteLine("No purchase history.");
                }
                Console.WriteLine();
            }
        }
    }

    static void MakePurchase(IDatabase database)
{
    Console.Write("Enter your username: ");
    string username = Console.ReadLine();
    User user = database.GetUser(username);

    // Create a new user if the entered username is not found
    if (user == null)
    {
        user = new User
        {
            Username = username,
            Balance = 0 //  balance for the new user
        };

        database.AddUser(user);
        Console.WriteLine($"New user '{username}' created.");
    }

    DisplayAvailableProducts(database);

    Console.Write("Enter the product ID you want to purchase: ");
    if (int.TryParse(Console.ReadLine(), out int productId))
    {
        Product product = database.GetProductById(productId);

        if (product == null)
        {
            Console.WriteLine("Invalid product ID. Can't find the product.");
            return;
        }

        Console.Write("Enter the quantity you want to purchase: ");
        if (int.TryParse(Console.ReadLine(), out int quantity))
        {
            if (quantity > product.Stock)
            {
                Console.WriteLine("Invalid quantity. Please enter a valid quantity.");
                return;
            }

            double totalCost = product.Price * quantity;

            if (user.Balance < totalCost)
            {
                Console.WriteLine($"Insufficient funds. You have ${user.Balance}, the total cost is ${totalCost}.");
                return;
            }

            if (product.Stock < quantity)
            {
                Console.WriteLine($"Not enough stock. Available stock: {product.Stock}");
                return;
            }

            user.Balance -= totalCost;
            product.Stock -= quantity;

            database.AddPurchase(user, product, quantity);

            Console.WriteLine($"{user.Username} purchased {quantity} {product.Name}(s) for ${totalCost}.");
        }
        else
        {
            Console.WriteLine("Invalid quantity. Please enter a valid number.");
        }
    }
    }

    static void RechargeBalance(IDatabase database)
    {
        RechargeOrCreateUser(database);

        Console.Write("Enter the amount to recharge: ");
        if (double.TryParse(Console.ReadLine(), out double rechargeAmount) && rechargeAmount > 0)
        {
            user.Balance += rechargeAmount;
            Console.WriteLine($"{user.Username}'s balance has been recharged by ${rechargeAmount}. Current balance: ${user.Balance}");
        }
        else
        {
            Console.WriteLine("Invalid recharge amount. Please enter a valid positive number.");
        }
    }

    static User user;

    static void RechargeOrCreateUser(IDatabase database)
    {
        Console.Write("Enter your username: ");
        string username = Console.ReadLine();
        user = database.GetUser(username);

        if (user == null)
        {
            user = new User
            {
                Username = username,
                Balance = 0
            };

            database.AddUser(user);
            Console.WriteLine($"New user '{username}' created.");
        }
    }

   
}