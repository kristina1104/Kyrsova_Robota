class User
{
    public string Username { get; set; }
    public double Balance { get; set; }
}

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public int Stock { get; set; }
}

class Purchase
{
    public User User { get; set; }
    public Product Product { get; set; }
    public int Quantity { get; set; }
    public DateTime PurchaseDate { get; set; }
}