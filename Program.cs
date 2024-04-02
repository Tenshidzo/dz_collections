namespace dz_collections
{
    public class Buyer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }

        public Buyer(int id, string name, int age)
        {
            Id = id;
            Name = name;
            Age = age;
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public int ProductId { get; set; }

        public Order(int id, int buyerId, int productId)
        {
            Id = id;
            BuyerId = buyerId;
            ProductId = productId;
        }
    }

    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Category(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Manufacturer
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Manufacturer(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public int OrderId { get; set; }

        public Product(int id, int categoryId, int manufacturerId, int orderId)
        {
            Id = id;
            CategoryId = categoryId;
            ManufacturerId = manufacturerId;
            OrderId = orderId;
        }
    }
    public class DBContext
    {
        private List<Buyer> buyers;
        private List<Order> orders;
        private List<Category> categories;
        private List<Manufacturer> manufacturers;
        private List<Product> products;

        public DBContext()
        {
            buyers = new List<Buyer>();
            orders = new List<Order>();
            categories = new List<Category>();
            manufacturers = new List<Manufacturer>();
            products = new List<Product>();
        }

        public void AddBuyer(Buyer buyer)
        {
            buyers.Add(buyer);
        }

        public void AddOrder(Order order)
        {
            orders.Add(order);
        }

        public void AddCategory(Category category)
        {
            categories.Add(category);
        }

        public void AddManufacturer(Manufacturer manufacturer)
        {
            manufacturers.Add(manufacturer);
        }

        public void AddProduct(Product product)
        {
            products.Add(product);
        }

        public List<Buyer> GetBuyers()
        {
            return buyers;
        }

        public List<Order> GetOrders()
        {
            return orders;
        }

        public List<Category> GetCategories()
        {
            return categories;
        }

        public List<Manufacturer> GetManufacturers()
        {
            return manufacturers;
        }

        public List<Product> GetProducts()
        {
            return products;
        }

        public List<Product> GetProductsByCategory(int categoryId)
        {
            return products.Where(p => p.CategoryId == categoryId).ToList();
        }

        public List<Product> GetProductsByManufacturer(int manufacturerId)
        {
            return products.Where(p => p.ManufacturerId == manufacturerId).ToList();
        }

        public List<Order> GetOrdersByBuyer(int buyerId)
        {
            return orders.Where(o => o.BuyerId == buyerId).ToList();
        }

        public List<Order> GetOrdersByProduct(int productId)
        {
            return orders.Where(o => o.ProductId == productId).ToList();
        }
        public string GetCategoryNameById(int categoryId)
        {
            var category = categories.FirstOrDefault(c => c.Id == categoryId);
            return category != null ? category.Name : "Unknown";
        }
    }
    public class Service
    {
        private DBContext dbContext;

        public Service(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateProduct(Product product)
        {
            dbContext.AddProduct(product);
        }

        public Product GetProductById(int id)
        {
            return dbContext.GetProducts().FirstOrDefault(p => p.Id == id);
        }

        public void UpdateProduct(Product product)
        {
            
            Product existingProduct = dbContext.GetProducts().FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Id = product.Id;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.ManufacturerId = product.ManufacturerId;
                existingProduct.OrderId = product.OrderId;
            }
        }

        public void DeleteProduct(int id)
        {
            Product product = dbContext.GetProducts().FirstOrDefault(p => p.Id == id);
            if (product != null)
            {
                dbContext.GetProducts().Remove(product);
            }
        }

        public List<Product> SearchProductsByCategoryId(int categoryId)
        {
            return dbContext.GetProducts().Where(p => p.CategoryId == categoryId).ToList();
        }
        public List<Product> SearchProducts(int categoryId, int manufacturerId)
        {
            return dbContext.GetProducts().Where(p => p.CategoryId == categoryId && p.ManufacturerId == manufacturerId).ToList();
        }
        public List<Product> SortProductsByManufacturerId()
        {
            return dbContext.GetProducts().OrderBy(p => p.ManufacturerId).ToList();
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            DBContext dbContext = new DBContext();
            Service service = new Service(dbContext);
            List<Buyer> buyers = new List<Buyer>
            {
                new Buyer(1, "Alex", 30),
                new Buyer(2, "Alina", 25)
            };

            List<Order> orders = new List<Order>
            {
                new Order(1, 1, 1),
                new Order(2, 1, 2),
                new Order(3, 2, 1)
            };

            List<Category> categories = new List<Category>
            {
                new Category(1, "Electronics"),
                new Category(2, "Clothing")
            };

            List<Manufacturer> manufacturers = new List<Manufacturer>
            {
                new Manufacturer(1, "Sony"),
                new Manufacturer(2, "Nike")
            };
            List<Product> products = new List<Product>
            {
                new Product(1, 1, 1, 1),
                new Product(2, 1, 2, 2),
                new Product(3, 2, 2, 3)
            };
            foreach (var buyer in buyers)
            {
                dbContext.AddBuyer(buyer);
            }

            foreach (var order in orders)
            {
                dbContext.AddOrder(order);
            }

            foreach (var category in categories)
            {
                dbContext.AddCategory(category);
            }

            foreach (var manufacturer in manufacturers)
            {
                dbContext.AddManufacturer(manufacturer);
            }

            foreach (var prod in products)
            {
                dbContext.AddProduct(prod);
            }
            //    foreach (var buyer in buyers)
            //    {
            //        Console.WriteLine($"Buyer: {buyer.Name}, Age: {buyer.Age}");
            //        var buyerOrders = orders.FindAll(o => o.BuyerId == buyer.Id);
            //        foreach (var order in buyerOrders)
            //        {
            //            var product = products.Find(p => p.Id == order.ProductId);
            //            var category = categories.Find(c => c.Id == product.CategoryId);
            //            var manufacturer = manufacturers.Find(m => m.Id == product.ManufacturerId);
            //            Console.WriteLine($"Order: {order.Id}, Product: {category.Name} ({manufacturer.Name})");
            //        }
            //        Console.WriteLine();
            //    }
            //List<Product> electronicsProducts = dbContext.GetProductsByCategory(1);
            //foreach (var product in electronicsProducts)
            //{
            //    Console.WriteLine($"Product ID: {product.Id}, Category: {dbContext.GetCategoryNameById(product.CategoryId)}");
            //}
            Product product = service.GetProductById(1);
            //if (product != null)
            //{
            //    Console.WriteLine($"Product ID: {product.Id}, CategoryId: {product.CategoryId}, ManufacturerId: {product.ManufacturerId}, OrderId: {product.OrderId}");
            //}
            service.UpdateProduct(new Product(1, 2, 2, 2));
            service.DeleteProduct(1);
            //List<Product> productsByCategory = service.SearchProductsByCategoryId(1);
            //foreach (var prod in productsByCategory)
            //{
            //    Console.WriteLine($"Product ID: {prod.Id}, CategoryId: {prod.CategoryId}, ManufacturerId: {prod.ManufacturerId}, OrderId: {prod.OrderId}");
            //}
            //List<Product> productsByCategoryAndManufacturer = service.SearchProducts(1, 2);
            //foreach (var prod in productsByCategoryAndManufacturer)
            //{
            //    Console.WriteLine($"Product ID: {prod.Id}, CategoryId: {prod.CategoryId}, ManufacturerId: {prod.ManufacturerId}, OrderId: {prod.OrderId}");
            //}
            List<Product> sortedProducts = service.SortProductsByManufacturerId();
            foreach (var prod in sortedProducts)
            {
                Console.WriteLine($"Product ID: {prod.Id}, CategoryId: {prod.CategoryId}, ManufacturerId: {prod.ManufacturerId}, OrderId: {prod.OrderId}");
            }

        }
    }
}
