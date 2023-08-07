using System.Text.Json.Serialization;
using System.Text.Json;
using System.Text;

//public class Bank
//{
//    public JsonSerializerOptions jsonOptions = new JsonSerializerOptions
//    {
//        WriteIndented = true,
//        Converters = { new JsonStringEnumConverter() }
//    };

//    public List<BankAccount> accounts = new List<BankAccount>();

//    public BankAccount GetAccountByNumber(string accountNumber)
//    {
//        return accounts.FirstOrDefault(a => a.AccountNumber == accountNumber);
//    }


//    public void OpenAccount(BankAccount newAccount)
//    {
//        accounts.Add(newAccount);

//        SaveAccountsState();

//    }

//    public void Deposit(BankAccount account, decimal amount)
//    {
//        account.Balance += amount;

//        SaveAccountsState();

//    }

//    public void Withdraw(BankAccount account, decimal amount)
//    {
//        if (account.Balance >= amount)
//        {
//            account.Balance -= amount;

//            SaveAccountsState();

//        }
//        else
//        {
//            Console.WriteLine("Недостаточно средств на счете.");
//        }
//    }

//    public void Transfer(BankAccount fromAccount, BankAccount toAccount, decimal amount)
//    {
//        if (fromAccount.Balance >= amount)
//        {
//            fromAccount.Balance -= amount;
//            toAccount.Balance += amount;

//            SaveAccountsState();
//        }
//        else
//        {
//            Console.WriteLine("Недостаточно средств на счете для перевода.");
//        }
//    }


//    public void PrintAccountInfo(BankAccount account)
//    {
//        Console.WriteLine($"Номер счета: {account.AccountNumber}, Владелец: {account.OwnerName}, Баланс: {account.Balance}");
//    }

//    public void SaveAccountsState()
//    {
//        string json = JsonSerializer.Serialize(accounts, jsonOptions);
//        File.WriteAllText("accounts.json", json, Encoding.UTF8);
//    }

//    public void LoadAccountsState()
//    {
//        if (File.Exists("accounts.json"))
//        {
//            string json;
//            using (StreamReader reader = new StreamReader("accounts.json", Encoding.UTF8))
//            {
//                json = reader.ReadToEnd();
//            }
//            accounts = JsonSerializer.Deserialize<List<BankAccount>>(json, jsonOptions);
//        }
//    }

//}