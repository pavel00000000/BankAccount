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
//Обеспечьте валидацию вводимых данных пользователя и информативные сообщения об ошибках (разбей задачу на полдзадачи)

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

Bank bank = new Bank();
bank.LoadAccountsState();

while (true)
{
    Console.Clear();
    Console.WriteLine("Меню:");
    Console.WriteLine("1 - Создать счет");
    Console.WriteLine("2 - Пополнить счет");
    Console.WriteLine("3 - Снять со счета");
    Console.WriteLine("4 - Перевести средства");
    Console.WriteLine("5 - Показать информацию о счете");
    Console.WriteLine("6 - Удалить счет");
    Console.WriteLine("7 - Выход");
    Console.Write("Введите номер команды: ");
    string command = Console.ReadLine();

    switch (command)
    {
        case "1":
            try 
            {  
            Console.Write("Введите номер счета: ");
            string accountNumber = Console.ReadLine();
            Console.Write("Введите имя владельца: ");
            string owner = Console.ReadLine();
            BankAccount newAccount = new BankAccount { AccountNumber = accountNumber, OwnerName = owner, Balance = 0 };
            bank.OpenAccount(newAccount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;
           
        case "2":
            try
            {
                Console.Write("Введите номер счета: ");
                string accountNumber = Console.ReadLine();
                Console.Write("Введите сумму для пополнения: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                BankAccount account = bank.GetAccountByNumber(accountNumber);
                if (account != null) bank.Deposit(account, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;

        case "3":
            try
            {
                Console.Write("Введите номер счета: ");
                string accountNumber = Console.ReadLine();
                Console.Write("Введите сумму для снятия: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                BankAccount account = bank.GetAccountByNumber(accountNumber);
                if (account != null) bank.Withdraw(account, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;

        case "4":
            try
            {
                Console.Write("Введите номер счета, с которого хотите перевести средства: ");
                string fromAccountNumber = Console.ReadLine();
                Console.Write("Введите номер счета, на который хотите перевести средства: ");
                string toAccountNumber = Console.ReadLine();
                Console.Write("Введите сумму для перевода: ");
                decimal amount = decimal.Parse(Console.ReadLine());
                BankAccount fromAccount = bank.GetAccountByNumber(fromAccountNumber);
                BankAccount toAccount = bank.GetAccountByNumber(toAccountNumber);
                if (fromAccount != null && toAccount != null) bank.Transfer(fromAccount, toAccount, amount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;

        case "5":
            try
            {
                Console.Write("Введите номер счета: ");
                string accountNumber = Console.ReadLine();
                BankAccount account = bank.GetAccountByNumber(accountNumber);
                if (account != null) bank.PrintAccountInfo(account);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;
        case "6":
            try
            {
                Console.Write("Введите номер счета, который хотите удалить: ");
                string accountNumber = Console.ReadLine();
                BankAccount accountToRemove = bank.GetAccountByNumber(accountNumber);

                if (accountToRemove != null)
                {
                    bool confirm = bank.ConfirmAction("Желаете удалить счет?");
                    if (confirm)
                    {
                        bank.RemoveAccount(accountToRemove);
                    }
                    else
                    {
                        Console.WriteLine("Удаление счета отменено.");
                    }
                }
                else
                {
                    Console.WriteLine("Счет с указанным номером не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            break;
        case "7":
            Environment.Exit(0);
            break;
        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }
    Console.WriteLine("Нажмите Enter, чтобы продолжить...");
    Console.ReadLine();
}

public class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public string OwnerName { get; set; }
}


public class Bank
{
    public JsonSerializerOptions jsonOptions = new JsonSerializerOptions
    {
        WriteIndented = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
    };

    public List<BankAccount> accounts = new List<BankAccount>();

    public BankAccount GetAccountByNumber(string accountNumber)
    {
        return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
    }


    public void OpenAccount(BankAccount newAccount)
    {
        accounts.Add(newAccount);

        SaveAccountsState();

    }

    public void Deposit(BankAccount account, decimal amount)
    {
        account.Balance += amount;

        SaveAccountsState();

    }

    public void Withdraw(BankAccount account, decimal amount)
{
    if (account.Balance >= amount)
    {
        account.Balance -= amount;
        SaveAccountsState();
    }
    else
    {
        Console.WriteLine("Недостаточно средств на счете для снятия.");
    }
}

    public void Transfer(BankAccount fromAccount, BankAccount toAccount, decimal amount)
    {
        if (fromAccount.Balance >= amount)
        {
            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            SaveAccountsState();
        }
        else
        {
            Console.WriteLine("Недостаточно средств на счете для перевода.");
        }
    }


    public void PrintAccountInfo(BankAccount account)
    {
        Console.WriteLine($"Номер счета: {account.AccountNumber}, Владелец: {account.OwnerName}, Баланс: {account.Balance}");
    }

    public void SaveAccountsState()
    {
        string json = JsonSerializer.Serialize(accounts, jsonOptions);
        File.WriteAllText("accounts.json", json, Encoding.UTF8);
    }

    public void LoadAccountsState()
    {
        if (File.Exists("accounts.json"))
        {
            string json;
            using (StreamReader reader = new StreamReader("accounts.json", Encoding.UTF8))
            {
                json = reader.ReadToEnd();
            }
            accounts = JsonSerializer.Deserialize<List<BankAccount>>(json, jsonOptions);
        }
    }

    public void RemoveAccount(BankAccount account)
    {
        bool confirm = ConfirmAction();
        if (confirm)
        {
            accounts.Remove(account);
            SaveAccountsState();
            Console.WriteLine("Счет успешно удален.");
        }
        else
        {
            Console.WriteLine("Удаление счета отменено.");
        }
    }

    public bool ConfirmAction(string message = "Вы уверены?")
    {
        while (true)
        {
            Console.Write($"{message} (Да/Нет): ");
            string input = Console.ReadLine().Trim().ToLower();

            if (input == "да")
            {
                return true;
            }
            else if (input == "нет")
            {
                return false;
            }
            else
            {
                Console.WriteLine("Пожалуйста, введите 'Да' или 'Нет'.");
                return ConfirmAction(message);
            }
        }
    }
}
