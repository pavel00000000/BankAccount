//Команда должна состоять из 2-3 человек.
//Создайте публичный репозиторий на GitHub для вашего проекта.
//Разработайте класс BankAccount, который будет представлять банковский счет с полями: номер счета, баланс и владелец счета.
//Создайте класс Bank, который будет отвечать за управление банковскими счетами: открытие новых счетов, пополнение счетов, списание средств, переводы между счетами и вывод информации о счетах.
//Реализуйте консольный интерфейс для взаимодействия с пользователями. Приложение должно предоставлять следующие команды:
//"Создать счет" - создание нового банковского счета с указанием номера счета и имени владельца.
//"Пополнить счет" - пополнение баланса счета по его номеру.
//"Снять со счета" - списание средств со счета по его номеру.
//"Перевести средства" - перевод денег с одного счета на другой с указанием номеров счетов.
//"Показать информацию о счете" - вывод информации о банковском счете по его номеру.
//"Выход" - завершение работы приложения.
//Каждое изменение в состоянии банковских счетов должно быть сохранено в файле (например, в формате List) с использованием библиотеки для работы с файлами, такой как System.IO.
//При запуске приложения, оно должно загружать ранее сохраненное состояние счетов из файла (если такой файл есть) и предоставлять возможность продолжить работу с ними.

//Бонусные задания:
//Реализуйте возможность удаления счетов.
//Добавьте проверку на недостаточность средств при списании.
//Обеспечьте валидацию вводимых данных пользователя и информативные сообщения об ошибках. разбей задачу на полдзадачи

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public interface IBank
{
    void OpenAccount(BankAccount newAccount);
    void Deposit(BankAccount account, decimal amount);
    void Withdraw(BankAccount account, decimal amount);
    void Transfer(BankAccount fromAccount, BankAccount toAccount, decimal amount);
    void PrintAccountInfo(BankAccount account);
}

public class BankAccount
{
    public string? AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public string? Owner { get; set; }
}

public class Bank : IBank
{
    public List<BankAccount> accounts = new List<BankAccount>();

    public void OpenAccount(BankAccount newAccount)
    {
        accounts.Add(newAccount);
    }

    public void Deposit(BankAccount account, decimal amount)
    {
        account.Balance += amount;
    }

    public void Withdraw(BankAccount account, decimal amount)
    {
        if (account.Balance >= amount)
        {
            account.Balance -= amount;
        }
        else
        {
            Console.WriteLine("Недостаточно средств на счете.");
        }
    }

    public void Transfer(BankAccount fromAccount, BankAccount toAccount, decimal amount)
    {
        if (fromAccount.Balance >= amount)
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;
        }
        else
        {
            Console.WriteLine("Недостаточно средств на счете для перевода.");
        }
    }

    public void PrintAccountInfo(BankAccount account)
    {
        Console.WriteLine($"Номер счета: {account.AccountNumber}, Владелец: {account.Owner}, Баланс: {account.Balance}");
    }

    // Метод для сохранения состояния счетов в файл
    public void SaveAccountsToFile(string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            foreach (var account in accounts)
            {
                writer.WriteLine($"{account.AccountNumber},{account.Balance},{account.Owner}");
            }
        }
    }

    // Метод для загрузки состояния счетов из файла
    public void LoadAccountsFromFile(string fileName)
    {
        if (File.Exists(fileName))
        {
            using (StreamReader reader = new StreamReader(fileName))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        BankAccount account = new BankAccount
                        {
                            AccountNumber = parts[0],
                            Balance = decimal.Parse(parts[1]),
                            Owner = parts[2]
                        };
                        accounts.Add(account);
                    }
                }
            }
        }
    }
}

public class Program
{
    public static void Main()
    {
        Bank bank = new Bank();
        string fileName = "bank_accounts.txt";

        // Загрузка состояния счетов из файла
        bank.LoadAccountsFromFile(fileName);

        // Консольный интерфейс для взаимодействия с пользователями
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("1. Создать счет");
            Console.WriteLine("2. Пополнить счет");
            Console.WriteLine("3. Снять со счета");
            Console.WriteLine("4. Перевести средства");
            Console.WriteLine("5. Показать информацию о счете");
            Console.WriteLine("6. Выход");
            Console.Write("Ваш выбор: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите номер счета: ");
                    string accountNumber = Console.ReadLine();
                    Console.Write("Введите имя владельца: ");
                    string owner = Console.ReadLine();
                    BankAccount newAccount = new BankAccount
                    {
                        AccountNumber = accountNumber,
                        Balance = 0.0m,
                        Owner = owner
                    };
                    bank.OpenAccount(newAccount);
                    Console.WriteLine("Счет успешно создан.");
                    break;
                case "2":
                    Console.Write("Введите номер счета для пополнения: ");
                    string accountNumberToDeposit = Console.ReadLine();
                    BankAccount accountToDeposit = bank.accounts.FirstOrDefault(acc => acc.AccountNumber == accountNumberToDeposit);
                    if (accountToDeposit != null)
                    {
                        Console.Write("Введите сумму для пополнения: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amountToDeposit))
                        {
                            bank.Deposit(accountToDeposit, amountToDeposit);
                            Console.WriteLine("Счет успешно пополнен.");
                        }
                        else
                        {
                            Console.WriteLine("Некорректная сумма.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Счет не найден.");
                    }
                    break;

                case "3":
                    Console.Write("Введите номер счета для снятия: ");
                    string accountNumberToWithdraw = Console.ReadLine();
                    BankAccount accountToWithdraw = bank.accounts.FirstOrDefault(acc => acc.AccountNumber == accountNumberToWithdraw);
                    if (accountToWithdraw != null)
                    {
                        Console.Write("Введите сумму для снятия: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal amountToWithdraw))
                        {
                            bank.Withdraw(accountToWithdraw, amountToWithdraw);
                            Console.WriteLine("Средства успешно сняты со счета.");
                        }
                        else
                        {
                            Console.WriteLine("Некорректная сумма.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Счет не найден.");
                    }
                    break;

                case "4":
                    Console.Write("Введите номер счета отправителя: ");
                    string fromAccountNumber = Console.ReadLine();
                    Console.Write("Введите номер счета получателя: ");
                    string toAccountNumber = Console.ReadLine();
                    BankAccount fromAccount = bank.accounts.FirstOrDefault(acc => acc.AccountNumber == fromAccountNumber);
                    BankAccount toAccount = bank.accounts.FirstOrDefault(acc => acc.AccountNumber == toAccountNumber);

                    if (fromAccount != null && toAccount != null)
                    {
                        Console.Write("Введите сумму для перевода: ");
                        if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount))
                        {
                            bank.Transfer(fromAccount, toAccount, transferAmount);
                            Console.WriteLine("Средства успешно переведены.");
                        }
                        else
                        {
                            Console.WriteLine("Некорректная сумма.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Счет отправителя или получателя не найден.");
                    }
                    break;
                case "5":
                    Console.Write("Введите номер счета: ");
                    string accountNumberToShow = Console.ReadLine();
                    BankAccount accountToShow = bank.accounts.FirstOrDefault(acc => acc.AccountNumber == accountNumberToShow);
                    if (accountToShow != null)
                    {
                        bank.PrintAccountInfo(accountToShow);
                    }
                    else
                    {
                        Console.WriteLine("Счет не найден.");
                    }
                    break;
                case "6":
                    // Сохранение состояния счетов в файл перед выходом
                    bank.SaveAccountsToFile(fileName);
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Некорректный выбор.");
                    break;
            }
        }
    }
}
