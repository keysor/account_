namespace Final_Project;
class Program
{
    public enum AccountType
    {
        Savings,
        Checking
    }
    public class Account
    {
    public Int64 AccountNumber { get; set; }
    public int Pin { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
    public int NumDeposits { get; set; }
    public int NumWithdrawals { get; set; }
    public AccountType AccountType { get; set; }
    public Account(string accountNumber, int pin, string firstName, string lastName, decimal balance, int numDeposits, int numWithdrawals, AccountType type)
    {
        AccountNumber = Int64.Parse(accountNumber);
        Pin = pin;
        FirstName = firstName;
        LastName = lastName;
        Balance = balance;
        NumDeposits = numDeposits;
        NumWithdrawals = numWithdrawals;
        AccountType AccountType = type;
    }

    }

    

    static void Main(string[] args)
    {
      while (true)
        {
            Console.WriteLine("Welcome to your Online Banking Application!");
            Console.WriteLine("1. Account Login");
            Console.WriteLine("2. Create Account");
            Console.WriteLine("3. Administrator Login");
            Console.WriteLine("4. Quit");
            Console.Write("Select Option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AccountLogin();
                    break;
                case "2":
                    CreateAccount();
                    break;
                case "3":
                    AdminLogin();
                    break;
                case "4":
                    Console.WriteLine("Exiting the application...");
                    return;
                default:
                    Console.WriteLine("**Invalid option.**");
                    break;

            }
        }
    }
    static void AccountLogin()
    {
        Console.Write("Enter account number: ");
        string accountNumber = Console.ReadLine();
        Console.Write("Enter PIN: ");
        string pin = Console.ReadLine();

        string[] lines = File.ReadAllLines("account_data.csv");

        foreach (string line in lines)
        {
            string[] linecsv = line.Split(',');
            if (linecsv[0] == accountNumber && linecsv[1] == pin)
            {
                Console.WriteLine("Login successful");
                AccountType accountType;
                if (!Enum.TryParse(linecsv[7], out accountType))
                {
                    Console.WriteLine("Invalid account type.");
                    return;
                }
                Account account = new Account(linecsv[0], int.Parse(linecsv[1]), linecsv[2], linecsv[3], decimal.Parse(linecsv[4]), int.Parse(linecsv[5]), int.Parse(linecsv[6]), accountType);
                AccountServiceMenu(account);
                return;
            }
        }
    Console.WriteLine("Invalid account number and/or PIN number");

    }
    private static void AccountServiceMenu(Account account)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("What would you like to do?\n");
            Console.WriteLine("1. Make a Withdrawal");
            Console.WriteLine("2. Make a Deposit");
            Console.WriteLine("3. Transfer Funds to another user Account");
            Console.WriteLine("4. Balance Inquiry");
            Console.WriteLine("5. Back to Main Menu");
            Console.WriteLine("\n Select an option: ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    Console.WriteLine("Enter the amount to deposit: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                    {
                        account.Balance += depositAmount;
                        account.NumDeposits++;
                        Console.WriteLine("Deposit successful, New balance: {0:C}", account.Balance);

                        string[] lines = File.ReadAllLines("account_data.csv");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] linecsv = lines[i].Split(',');
                            if (linecsv[0] == account.AccountNumber.ToString())
                            {
                              
                                linecsv[4] = account.Balance.ToString();
                                linecsv[5] = account.NumDeposits.ToString();
                                lines[i] = string.Join(",", linecsv);
                                File.WriteAllLines("account_data.csv", lines);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. You have: {0:C}", account.Balance);
                    }
                    break;
                case "2":
                    Console.WriteLine("Enter withdraw amount: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal withdrawAmount) || withdrawAmount < account.Balance)
                    {
                        account.Balance -= withdrawAmount;
                        account.NumWithdrawals++;
                        Console.WriteLine("Withdraw successful, new balance: {0:C}", account.Balance);

                        string[] lines = File.ReadAllLines("account_data.csv");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] linecsv = lines[i].Split(',');
                            if (linecsv[0] == account.AccountNumber.ToString())
                            {
                              
                                linecsv[4] = account.Balance.ToString();
                                linecsv[6] = account.NumWithdrawals.ToString();
                                lines[i] = string.Join(",", linecsv);
                                File.WriteAllLines("account_data.csv", lines);
                                break;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. You have: {0:C}", account.Balance);
                    }
                    break;
                case "3":
                    Console.WriteLine("How much would you like to transfer?: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal transferammount) || transferammount < account.Balance)
                    {
                        
                        Console.WriteLine("Enter account number of person you would like to transfer to: ");
                        string recipient = Console.ReadLine();
                        string[] lines = File.ReadAllLines("account_data.csv");
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] linecsv = lines[i].Split(',');
                            if (linecsv[0] == recipient)
                            {
                                account.Balance =- transferammount;
                                Decimal recipientBalance = Decimal.Parse(linecsv[4]);
                                recipientBalance =+ transferammount;
                                linecsv[4] = recipientBalance.ToString();
                                lines[i] = string.Join(",", linecsv);
                                File.WriteAllLines("account_data.csv", lines);
                            }
                            else
                            {
                                Console.WriteLine("Invalid account number");
                            }
                            if (linecsv[0] == account.AccountNumber.ToString())
                            {
                                linecsv[4] = account.Balance.ToString();
                            }
                            else
                            {
                            }
                            
                        }
                    }
                    else
                    {
                        Console.WriteLine("You have insufficient funds. You ahve {0:C}", account.Balance);
                    }
                    break;
                case "4":
                    Console.WriteLine("**Your balance is: {0:C}**", account.Balance);
                    break;
                case "5":
                    exit = true;
                    return;
                default:
                    Console.WriteLine("Invalid choice.");
                    break;
            }


        }




    }

    static void CreateAccount()
    {
        Console.WriteLine("First Name: ");
        string firstName = Console.ReadLine();
        Console.WriteLine("Last Name: ");
        string lastName = Console.ReadLine();
        Console.WriteLine("Enter PIN code (4 Digits): ");
        string pin = Console.ReadLine();
        int pinNumber;
        bool pSuccess = int.TryParse(pin, out pinNumber);
        if (!pSuccess || pinNumber < 1000 || pinNumber > 9999)
        {
            Console.WriteLine("Invalid Pin, Must be a 4-Digit number.");
            return;
        }
        Console.WriteLine("Choose an account type:");
        Console.WriteLine("1. Savings");
        Console.WriteLine("2. Checking");
        string accountType = Console.ReadLine();
        AccountType type;
        switch (accountType)
        {
            case "1":
                type = AccountType.Savings;
                break;
            case "Savings":
                type = AccountType.Savings;
                break;
            case "2":
                type = AccountType.Checking;
                break;
            case "Checking":
                type = AccountType.Checking;
                break;
            default:
                Console.WriteLine("Invalid input, Choose 1 or 2. Or type \"Savings\" or \"Checking\"");
                return;
        }
        Random random = new Random();
        string accountNumber = "183977" + random.Next(1000000000, 2147483647).ToString();
        Account account = new Account(accountNumber, pinNumber, firstName, lastName, 100, 1, 0, type);


        using (StreamWriter writer = new StreamWriter("account_data.csv", true)) // write to the .txt
        {
            writer.WriteLine($"{account.AccountNumber},{account.Pin},{account.FirstName},{account.LastName},{account.Balance},{account.NumDeposits},{account.NumWithdrawals},{account.AccountType}");
        }

        Console.WriteLine($"Account created successfully with account number {accountNumber}");


    }
    static void AdminLogin()
    {
        Console.WriteLine("Administrator login selected");
        Console.WriteLine("Username: ");
        string username = Console.ReadLine();
        Console.WriteLine("Password: ");
        string password = Console.ReadLine();

        string[] lines = File.ReadAllLines("admins.txt");
            for (int i = 0; i < lines.Length; i++)
            {
            string[] linecsv = lines[i].Split(',');
                if (username == linecsv[0] || password == linecsv[1])
                {
                    ReportReview();
                }
                else
                {
                    Console.WriteLine("Username or Password is incorrect.");
                }
            }
            
    }
    static void ReportReview()
    {
        Console.WriteLine("1. Show average Savings Account balance");
        Console.WriteLine("2. Show total Savings Account balance");
        Console.WriteLine("3. Show average Checking Account balance");
        Console.WriteLine("4. Show total Checking Account balance");
        Console.WriteLine("5. Show the number of accounts for each account type");
        Console.WriteLine("6. Show the 10 accounts with the most deposits");
        Console.WriteLine("7. Show the 10 accounts with the most withdrawals");
        Console.WriteLine("8. Back to main menu");
        Console.WriteLine("\n Select Option: \n");
        string choice = Console.ReadLine();

        StreamReader reader = new StreamReader("account_data.csv");
        List<Account> accounts = new List<Account>();
        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();
            string[] parts = line.Split(',');
            string accountNumber = parts[0];
            int pin = int.Parse(parts[1]);
            string firstName = parts[2];
            string lastName = parts[3];
            decimal balance = decimal.Parse(parts[4]);
            int numDeposits = int.Parse(parts[5]);
            int numWithdrawals = int.Parse(parts[6]);
            AccountType accountType = (AccountType)Enum.Parse(typeof(AccountType), parts[7]);
            Account account = new Account(accountNumber, pin, firstName, lastName, balance, numDeposits, numWithdrawals, accountType);
            accounts.Add(account);
        }
        reader.Close();

        decimal totalSavingsBalance = 0;
        decimal totalCheckingBalance = 0;
        int numSavingsAccounts = 0;
        int numCheckingAccounts = 0;
        List<Account> topDepositAccounts = accounts.OrderByDescending(a => a.NumDeposits).Take(10).ToList();
        List<Account> topWithdrawalAccounts = accounts.OrderByDescending(a => a.NumWithdrawals).Take(10).ToList();

        foreach (Account account in accounts)
        {
            if (account.AccountType == AccountType.Savings)
            {
                totalSavingsBalance += account.Balance;
                numSavingsAccounts++;
            }
            else if (account.AccountType == AccountType.Checking)
            {
                totalCheckingBalance += account.Balance;
                numCheckingAccounts++;
            }
        }
        decimal avgSavingsBalance = numSavingsAccounts > 0 ? totalSavingsBalance / numSavingsAccounts : 0;
        decimal avgCheckingBalance = numCheckingAccounts > 0 ? totalCheckingBalance / numCheckingAccounts : 0;

        int numAccounts = accounts.Count;
        numSavingsAccounts = accounts.Count(a => a.AccountType == AccountType.Savings);
        numCheckingAccounts = accounts.Count(a => a.AccountType == AccountType.Checking);

        switch (choice)
        {
            case "1":
                Console.WriteLine("Average Savings Account Balance:  "+ avgSavingsBalance);
                break;
            case "2":
                Console.WriteLine("Total Savings Balance: " + totalSavingsBalance);
                break;
            case "3":
                Console.WriteLine("Average Checking Account Balance: " + avgCheckingBalance);
                break;
            case "4":
                Console.WriteLine("Total Checking Account Balance: " + totalCheckingBalance);
                break;
            case "5":
                Console.WriteLine("Savings Accounts: " + numSavingsAccounts);
                Console.WriteLine("Checking Accounts: " + numCheckingAccounts);
                break;
            case "6":
                Console.WriteLine("Top 10 in deposits");
                break;
            case "7":
                Console.WriteLine("Top 10 in Withdrawals");
                break;
            case "8":
                Console.WriteLine("Exciting to menu.");
                return;
            default:
                Console.WriteLine("Invalid choice");
                break;
        }

    }
}
