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
using System.Text.Json;


Bank bank = new Bank();
bank.LoadAccountsState();

while (true)
{
    Console.WriteLine("1 - Создать счет");
    Console.WriteLine("2 - Пополнить счет");
    Console.WriteLine("3 - Снять со счета");
    Console.WriteLine("4 - Перевести средства");
    Console.WriteLine("5 - Показать информацию о счете");
    Console.WriteLine("6 - Выход");
    Console.Write("Введите номер команды: ");
    string command = Console.ReadLine();

    switch (command)
    {
        case "1":
            Console.Write("Введите номер счета: ");
            string accountNumber = Console.ReadLine();
            Console.Write("Введите имя владельца: ");
            string owner = Console.ReadLine();
            BankAccount newAccount = new BankAccount { AccountNumber = accountNumber, Owner = owner, Balance = 0 };
            bank.OpenAccount(newAccount);
            break;

        case "2":
            Console.Write("Введите номер счета: ");
            accountNumber = Console.ReadLine();
            Console.Write("Введите сумму для пополнения: ");
            decimal amount = decimal.Parse(Console.ReadLine());
            BankAccount account = bank.GetAccountByNumber(accountNumber);
            if (account != null) bank.Deposit(account, amount);
            break;

        case "3":
            Console.Write("Введите номер счета: ");
            accountNumber = Console.ReadLine();
            Console.Write("Введите сумму для снятия: ");
            amount = decimal.Parse(Console.ReadLine());
            account = bank.GetAccountByNumber(accountNumber);
            if (account != null) bank.Withdraw(account, amount);
            break;

        case "4":
            Console.Write("Введите номер счета, с которого хотите перевести средства: ");
            string fromAccountNumber = Console.ReadLine();
            Console.Write("Введите номер счета, на который хотите перевести средства: ");
            string toAccountNumber = Console.ReadLine();
            Console.Write("Введите сумму для перевода: ");
            amount = decimal.Parse(Console.ReadLine());
            BankAccount fromAccount = bank.GetAccountByNumber(fromAccountNumber);
            BankAccount toAccount = bank.GetAccountByNumber(toAccountNumber);
            if (fromAccount != null && toAccount != null) bank.Transfer(fromAccount, toAccount, amount);
            break;

        case "5":
            Console.Write("Введите номер счета: ");
            accountNumber = Console.ReadLine();
            account = bank.GetAccountByNumber(accountNumber);
            if (account != null) bank.PrintAccountInfo(account);
            break;

        case "6":
            Environment.Exit(0);
            break;

        default:
            Console.WriteLine("Неизвестная команда.");
            break;
    }
}


public class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public string Owner { get; set; }
}

public class Bank
{
    private List<BankAccount> accounts = new List<BankAccount>();

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
            Console.WriteLine("Недостаточно средств на счете.");
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
        Console.WriteLine($"Номер счета: {account.AccountNumber}, Владелец: {account.Owner}, Баланс: {account.Balance}");
    }

    public void SaveAccountsState()
    {
        string json = JsonSerializer.Serialize(accounts);
        File.WriteAllText("accounts.json", json);
    }

    public void LoadAccountsState()
    {
        if (File.Exists("accounts.json"))
        {
            string json = File.ReadAllText("accounts.json");
            accounts = JsonSerializer.Deserialize<List<BankAccount>>(json);
        }
    }



}