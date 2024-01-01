interface IDatabase
{
    void AddUser(User user);
    User GetUser(string username);
    void AddPurchase(User user, Product product, int quantity);
    List<Purchase> GetPurchaseHistory(User user);
    List<Product> GetProducts();
    void AddProduct(Product product);
    Product GetProductById(int productId);
    List<User> GetUserList();
}