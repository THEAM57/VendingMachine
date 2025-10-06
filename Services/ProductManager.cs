namespace Services
{
    class ProductManager
    {
        private static List<Product> productsList = new List<Product>();
        
        public static void DisplayProducts()
        {
            FinanceManager.ShowBalance();
            
            if (productsList.Count == 0)
            {
                Console.WriteLine("Товаров нет!");
                return;
            }
            
            Console.WriteLine("Доступные товары:\n");
            
            for (int i = 0; i < productsList.Count; i++)
            {

                Product product = productsList[i];
                Console.WriteLine($"{i + 1}. {product.name} - {product.price} руб. (Осталось: {product.quantity})");
            }
        }
        
        public static void AddNewProduct()
        {
            Console.WriteLine("---ДОБАВЛЕНИЕ ТОВАРА---");
            try
            {
                Console.WriteLine("Введите название товара: ");
                string name = Console.ReadLine();
                
                Console.WriteLine("Введите цену товара: ");
                decimal price = decimal.Parse(Console.ReadLine());
                
                Console.WriteLine("Введите количество товара: ");
                int quantity = int.Parse(Console.ReadLine());
                
                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Ошибка: Название товара не может быть пустым!");
                    return;
                }
                
                if (price <= 0)
                {
                    Console.WriteLine("Ошибка: Цена должна быть положительной!");
                    return;
                }
                
                if (quantity < 0)
                {
                    Console.WriteLine("Ошибка: Количество не может быть отрицательным!");
                    return;
                }
                
                Product newProduct = new Product(name, price, quantity);
                productsList.Add(newProduct);
                
                Console.WriteLine($"\nТовар '{name}' добавлен!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
        }
        
        public static void BuyProduct()
        {
            Console.WriteLine("--- ПОКУПКА ТОВАРА ---");
            DisplayProducts();
            if (productsList.Count == 0)
            {
                return;
            }
        
            Console.Write("Введите номер товара: ");
            if (int.TryParse(Console.ReadLine(), out int productIndex))
            {
                if (productIndex < 1 || productIndex > productsList.Count)
                {
                    Console.WriteLine("Ошибка: Неверный номер товара!");
                    return;
                }
                Product product = productsList[productIndex - 1];
        
                if (product.quantity <= 0)
                {
                    Console.WriteLine("Ошибка: Этот товар закончился!");
                    return;
                }

                if (FinanceManager.GetBalance() < product.price)
                {
                    Console.WriteLine($"Ошибка: Недостаточно средств! Нужно {product.price} руб., на балансе {FinanceManager.GetBalance()} руб.");
                    return;
                }
            
                if (FinanceManager.ProcessPurchase(product.price))
                {
                    product.quantity--;
                    Console.WriteLine($" Успешно куплено: {product.name} за {product.price} руб.");
                }
            }
            else
            {
                Console.WriteLine("Ошибка: Введите корректный номер товара!");
            }
        }
        
        private class Product
        {
            public string name { get; set; }
            public decimal price { get; set; }
            public int quantity { get; set; }

            public Product(string name, decimal price, int quantity)
            {
                this.name = name;
                this.price = price;
                this.quantity = quantity;
            }
        }
    }
}    