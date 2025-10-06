using System;
using System.Collections.Generic;
using Services;

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
            Console.WriteLine("\nДо скорых встреч!\n");
            Environment.Exit(0);
        }
    }
}
