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

public class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public string Owner { get; set; }
}

public class Bank
{
    private List<BankAccount> accounts = new List<BankAccount>();

    public void OpenAccount(BankAccount newAccount)
    {
        accounts.Add(newAccount);
        // Здесь нужно вызвать функцию сохранения состояния счетов
    }

    public void Deposit(BankAccount account, decimal amount)
    {
        account.Balance += amount;
        // Здесь нужно вызвать функцию сохранения состояния счетов
    }

    public void Withdraw(BankAccount account, decimal amount)
    {
        if (account.Balance >= amount)
        {
            account.Balance -= amount;
            // Здесь нужно вызвать функцию сохранения состояния счетов
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
            // Здесь нужно вызвать функцию сохранения состояния счетов
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

    // Здесь должны быть реализованы функции загрузки и сохранения состояния счетов
}
