using System;
using System.Collections.Generic;

class Program
{
    public static bool userIsAdmin = false;
    public static string adminPassword = "admin239";
    
    static void Main(string[] args)
    {
        Console.WriteLine("Добро пожаловать в автомат!");
        
        while (true)
        {
            if (!userIsAdmin)
            {
                Console.WriteLine("---VENDING MACHINE---\n");
                Console.WriteLine("1. Показать товары");
                Console.WriteLine("2. Внести монеты");
                Console.WriteLine("3. Купить товар");
                Console.WriteLine("4. Вернуть деньги/получить сдачу");
                Console.WriteLine("5. Показать количество монет");
                Console.WriteLine("6. Режим администратора");
                Console.WriteLine("0. Выход\n");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ProductManager.DisplayProducts();
                        break;
                    case "2":
                        FinanceManager.InsertCoins();
                        break;
                    case "3":
                        ProductManager.BuyProduct();
                        break;
                    case "4":
                        FinanceManager.CalculateChange();
                        break;
                    case "5":
                        FinanceManager.ShowBalance();
                        break;
                    case "6":
                        ChangeRole();
                        break;
                    case "0":
                        ExitFromProgramm();
                        break;
                    default:
                        Console.WriteLine("Ошибка");
                        break;
                }
            }
            else
            {
                Console.WriteLine("---VENDING MACHINE---\n (Режим Администратора)\n");
                Console.WriteLine("\n1. Пополнить товары");
                Console.WriteLine("2. Собрать монеты");
                Console.WriteLine("3. Режим пользователя");
                
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        ProductManager.AddNewProduct();
                        break;
                    case "2":
                        FinanceManager.GetMachineRevenue();
                        break;
                    case "3":
                        Console.WriteLine("3. Режим пользователя");
                        ChangeRole();
                        break;
                }
            }
        }
    }

    private static void ChangeRole()
    {
        if (userIsAdmin)
        {
            userIsAdmin = false;
        }
        else
        {
            Console.WriteLine("Введите пароль администратора: ");
            if (Console.ReadLine() == Program.adminPassword)
            {
                userIsAdmin = true;
            }
            else
            {
                Console.WriteLine("Неверный пароль");
            }
        }
    }
    
    private static void ExitFromProgramm()
    {
        Console.WriteLine("Вы уверены, что хотите выйти? \n1. Да \n2. Нет");
        if (Console.ReadLine() == "1")
        {
            FinanceManager.CalculateChange();
            Console.WriteLine("\nДо скорых встреч!\n");
            Environment.Exit(0);
        }
    }
}

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

class FinanceManager
{
    private static decimal currentBalance = 0;
    private static decimal machineRevenue = 0;

    public static void ShowBalance()
    {
        Console.WriteLine($"Ваш баланс: {currentBalance}\n");
    }
    
    public static decimal GetBalance()
    {
        return currentBalance;
    }
    
    public static bool ProcessPurchase(decimal price)
    {
        if (currentBalance >= price)
        {
            currentBalance -= price;
            machineRevenue += price;
            return true;
        }
        return false;
    }
    
    public static void InsertCoins()
    {
        Console.WriteLine("\n--- ВНЕСЕНИЕ МОНЕТ ---");
        Console.WriteLine("Доступные номиналы: 1, 2, 5, 10 руб.");
        Console.WriteLine("Введите сумму для внесения: ");

        if (decimal.TryParse(Console.ReadLine(), out decimal amount))
        {
            if (amount == 1 || 
                amount == 2 ||
                amount == 5 ||
                amount == 10)
            {
                currentBalance += amount;
                Console.WriteLine($"Внесено {amount} монет\n");
            }
            else
            {
                Console.WriteLine("Ошибка: Автомат не принимает данные монеты");
            }
        }
        else
        {
            Console.WriteLine("Ошибка: Введите число!");
        }
    }
    
    public static void CalculateChange()
    {
        Console.WriteLine($"Ваша сдача - {currentBalance} руб.\n");
        currentBalance = 0;
    }

    public static void GetMachineRevenue()
    {
        Console.WriteLine($"Ваша выручка - {machineRevenue} руб.\nСобрать выручку?\n1.Да\n2.Нет\n");
        if (Console.ReadLine() == "1")
        {
            Console.WriteLine($"Собрано {machineRevenue} руб.");
            machineRevenue = 0;
        }
    }
}