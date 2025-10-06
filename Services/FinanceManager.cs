namespace Services
{
    class FinanceManager
    {
        private static Dictionary<decimal, int> machineCoins = new Dictionary<decimal, int>
        {
            {1, 100}, {2, 100}, {5, 50}, {10, 50}
        };
        
        private static Dictionary<decimal, int> userCoins = new Dictionary<decimal, int>
        {
            {1, 0}, {2, 0}, {5, 0}, {10, 0}
        };
        
        private static decimal currentBalance = 0;
        private static decimal machineRevenue = 0;

        public static void ShowBalance()
        {
            Console.WriteLine($"Ваш баланс: {currentBalance} руб.\n");
        }
        
        public static decimal GetBalance()
        {
            return currentBalance;
        }
        
        public static void InsertCoins()
        {
            Console.WriteLine("\n--- ВНЕСЕНИЕ МОНЕТ ---");
            Console.WriteLine("Выберите номинал монеты:");
            
            decimal[] denominations = { 1, 2, 5, 10 };
            
            for (int i = 0; i < denominations.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {denominations[i]} руб.");
            }
            
            Console.Write("Выберите номер номинала: ");
            if (int.TryParse(Console.ReadLine(), out int choice) && choice >= 1 && choice <= denominations.Length)
            {
                decimal selectedCoin = denominations[choice - 1];
                userCoins[selectedCoin]++;
                currentBalance += selectedCoin;
                
                Console.WriteLine($"Внесена монета: {selectedCoin} руб.");
                ShowBalance();
                
                Console.WriteLine("Внесенные монеты:");
                foreach (var coin in userCoins)
                {
                    if (coin.Value > 0)
                    {
                        Console.WriteLine($"  {coin.Key} руб. - {coin.Value} шт.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Неверный выбор!");
            }
        }
        
        public static bool ProcessPurchase(decimal price)
        {
            if (currentBalance >= price)
            {
                foreach (var coin in userCoins)
                {
                    if (coin.Value > 0)
                    {
                        machineCoins[coin.Key] += coin.Value;
                    }
                }
                
                decimal change = currentBalance - price;
                currentBalance = 0;
                
                userCoins = new Dictionary<decimal, int> { {1, 0}, {2, 0}, {5, 0}, {10, 0} };
                
                machineRevenue += price;
                
                if (change > 0)
                {
                    Console.WriteLine($"Ваша сдача: {change} руб.");
                    return TryGiveChange(change);
                }
                
                return true;
            }
            return false;
        }
        
        public static void CalculateChange()
        {
            if (currentBalance == 0)
            {
                Console.WriteLine("Нет денег для возврата!");
                return;
            }
            
            Console.WriteLine($"\n--- ВОЗВРАТ ДЕНЕГ---");
            
            Console.WriteLine("Возвращенные монеты:");
            foreach (var coin in userCoins)
            {
                if (coin.Value > 0)
                {
                    Console.WriteLine($"  {coin.Key} руб. - {coin.Value} шт.");
                }
            }
            
            currentBalance = 0;
            userCoins = new Dictionary<decimal, int> { {1, 0}, {2, 0}, {5, 0}, {10, 0} };
            Console.WriteLine("Деньги возвращены!");
        }

        private static bool TryGiveChange(decimal amount)
        {
            decimal remaining = amount;
            Dictionary<decimal, int> changeCoins = new Dictionary<decimal, int> { {10, 0}, {5, 0}, {2, 0}, {1, 0} };
            
            decimal[] denominations = { 10, 5, 2, 1 };
            
            foreach (decimal coin in denominations)
            {
                if (remaining >= coin && machineCoins[coin] > 0)
                {
                    int needed = (int)(remaining / coin);
                    int available = Math.Min(needed, machineCoins[coin]);
                    
                    if (available > 0)
                    {
                        changeCoins[coin] = available;
                        remaining -= coin * available;
                        machineCoins[coin] -= available;
                    }
                }
            }
            
            if (remaining == 0)
            {
                Console.WriteLine("Сдача выдана:");
                foreach (var coin in changeCoins)
                {
                    if (coin.Value > 0)
                    {
                        Console.WriteLine($"  {coin.Key} руб. - {coin.Value} шт.");
                    }
                }
                return true;
            }
            else
            {
                foreach (var coin in changeCoins)
                {
                    machineCoins[coin.Key] += coin.Value;
                }
                
                Console.WriteLine("Извините, в автомате недостаточно монет для выдачи сдачи!");
                Console.WriteLine("Будет произведен возврат внесенных денег.");

                CalculateChange();
                return false;
            }
        }

        public static void GetMachineRevenue()
        {
            if (!Program.userIsAdmin)
            {
                Console.WriteLine("Недостаточно прав!");
                return;
            }
            
            Console.WriteLine("\n--- СБОР ВЫРУЧКИ ---");

            decimal totalCoinsValue = 0;
            foreach (var coin in machineCoins)
            {
                totalCoinsValue += coin.Key * coin.Value;
            }
            
            Console.WriteLine($"Накопленная выручка: {machineRevenue} руб.");
            Console.WriteLine($"Монеты в автомате: {totalCoinsValue} руб.");
            Console.WriteLine($"Общая сумма: {machineRevenue + totalCoinsValue} руб.");
            
            Console.WriteLine("\nМонеты в автомате:");
            foreach (var coin in machineCoins)
            {
                if (coin.Value > 0)
                {
                    Console.WriteLine($"  {coin.Key} руб. - {coin.Value} шт.");
                }
            }
            
            Console.Write("\nСобрать выручку? (1 - Да, 2 - Нет): ");
            if (Console.ReadLine() == "1")
            {
                decimal collectedAmount = machineRevenue + totalCoinsValue;
                machineRevenue = 0;
                
                machineCoins[1] = 20;
                machineCoins[2] = 20;
                machineCoins[5] = 10;
                machineCoins[10] = 10;
                
                Console.WriteLine($"Собрано {collectedAmount} руб.");
                Console.WriteLine("Оставлен минимальный запас монет для выдачи сдачи.");
            }
        }
    }
}