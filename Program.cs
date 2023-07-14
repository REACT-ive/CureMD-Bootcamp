using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment_2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bank HBL_Accounts_Database = new Bank();

            SavingsAccount Hashims_oldage_account = new SavingsAccount(235200, "Muhammad Khan", 0, 0.05m);
            HBL_Accounts_Database.AddAccount(Hashims_oldage_account);

            CheckingAccount Hashims_Current_Account = new CheckingAccount(861976, "Muhammad Hashim", 0);
            HBL_Accounts_Database.AddAccount(Hashims_Current_Account);

            LoanAccount Hashims_startup_investment = new LoanAccount(0852016, "Muhammad Hashim", 0, 1000);
            HBL_Accounts_Database.AddAccount(Hashims_startup_investment);

            HBL_Accounts_Database.DepositToAccount(235200, 68000);
            HBL_Accounts_Database.DepositToAccount(861976, 20000);
            HBL_Accounts_Database.DepositToAccount(852016, 100000);

            Hashims_oldage_account.CalculateInterest();

            HBL_Accounts_Database.WithdrawFromAccount(235200, 30000);
            HBL_Accounts_Database.WithdrawFromAccount(861976, 5000);
            HBL_Accounts_Database.WithdrawFromAccount(852016, 20000);

            HBL_Accounts_Database.GrantLoan();

            Console.ReadKey();
        }
    }

    //Interface is the bridge between software programmers and the people who consume
    //their products. Below Interface will enable bank accountants to execute same commands
    //regardless of the bank account type.

    interface ITransaction
    {
        void ExecuteTransaction(decimal amount);
        void PrintTransaction();
    }

    //All Bank accounts will inherit properties from a single entity in order to keep the code
    //lean and readable.

    public abstract class BankAccount
    {
        //A class member is by default public, and that is why we are not specifying the file type
        protected string Account_holder_name { get; set; }
        public int Account_number { get; protected set; }
        protected decimal Balance { get; set; }

        //We can see that above data has been abstracted. User may have to enter/access such 
        //details only when they must have to do so. The above private datatypes will be accessed via
        //public methods.

        //Now that a few attributes -that are common between all bank accounts- have been
        //set, we will now create a constructor for this class.
        public BankAccount(string account_holder_name, int account_number, decimal balance)
        {
            Account_holder_name = account_holder_name;
            Account_number = account_number;
            Balance = balance;
        }

        public virtual void CalculateInterest()
        {
        }

        //In the parent class of BankAccount, overloading is about to be demonstrated. Overloading refers to
        //defining same method again and again with varying numbers of parameters being passed through it.
        //This is also referred to as Dynamic / Run-time Polymorphism since the decision making of choosing 
        //the correct code block is done during run/debugging time.

        public virtual void Deposit()
        {
            Console.WriteLine("How much amount has to be deposited?");
            string amount_string = Console.ReadLine();
            decimal amount = decimal.Parse(amount_string);

            Balance += amount;

            Console.WriteLine("An amount of " + amount + " has been deposited successfully. New Balance is =" + Balance);
        }

        public virtual void Deposit(decimal amount)
        {
            Balance += amount;
            Console.WriteLine("Your New Account Balance is " + Balance);
        }

        public virtual void Withdraw()
        {
            Console.WriteLine("How much amount has to be deducted?");
            string amount_string = Console.ReadLine();
            decimal amount = decimal.Parse(amount_string);
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine("An amount of " + amount + " has been deducted successfully. New Balance is =" + Balance);
            }
            else
            {
                Console.WriteLine("Insufficient funds...");
            }
        }

        public virtual void Withdraw(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                Console.WriteLine("An amount of " + amount + " has been deducted successfully. New Balance is =" + Balance);
            }
            else
            {
                Console.WriteLine("Insufficient funds...");
            }
        }
        public abstract void ExecuteTransaction(decimal amount);
        public abstract void PrintTransaction();

    }


    //Inheritance will be demonstrated. Savings account will inherit all methods and non-private members of
    //the parent class BankAccount
    class SavingsAccount : BankAccount, ITransaction
    {
        decimal Interest_rate;
        decimal Total_interest = 0;
        public SavingsAccount(int account_number, string account_holder_Name, decimal balance, decimal interest_rate)
            : base(account_holder_Name, account_number, balance)
        {
            Interest_rate = interest_rate;
        }

        //Following two methods must be implemented because of the Interface's requirements.
        public override void ExecuteTransaction(decimal amount)
        {
            Console.WriteLine("Write + for a deposit and - for a withdrawl");
            string Transaction = Console.ReadLine();

            if (Transaction == "+")
            {
                Balance += amount;
                Console.WriteLine("Transaction successful");
            }

            if (Transaction == "-")
            {
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine("Transaction successful");
                }
                else
                {
                    Console.WriteLine("Transaction unsuccessful");
                }
            }
        }
        public override void PrintTransaction()
        {
            Console.WriteLine("Write + for a deposit and - for a withdrawl");
            string Transaction = Console.ReadLine();

            if (Transaction == "+")
            {
                Console.WriteLine("How much amount has to be deposited?");
                string amount_string = Console.ReadLine();
                decimal amount = decimal.Parse(amount_string);

                Balance += amount;
                Console.WriteLine($"DEPOSIT of {amount} successful. New Balance: {Balance}");
            }

            if (Transaction == "-")
            {
                Console.WriteLine("How much amount has to be deducted?");
                string amount_string = Console.ReadLine();
                decimal amount = decimal.Parse(amount_string);
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine($"WITHDRAWAL of {amount} successful. New Balance: {Balance}");
                }
                else
                {
                    Console.WriteLine($"Insufficient funds... Total Balance = {Balance}");
                }
            }
        }
        //CalculateInterest() method must be overridden in all subclasses as instructed in the task.
        //Subclasses being able to override the the code blocks of parent classes with their own code blocks
        //is also referred as Static / Complie-time Polymorphism.

        public override void Deposit(decimal amount)
        {
            Total_interest = Total_interest + (amount * Interest_rate);
            Balance = amount + amount * Interest_rate;
            Console.WriteLine("Your New Account Balance is " + Balance);
        }

        public override void CalculateInterest()
        {
            decimal interest_rate = Interest_rate;
            Console.WriteLine($"Total Interest accumulated inside {Account_holder_name} Saving accounts is {Total_interest}");
        }
    }
    class CheckingAccount : BankAccount, ITransaction
    {
        public CheckingAccount(int account_number, string account_holder_Name, decimal balance)
            : base(account_holder_Name, account_number, balance)
        {
        }

        public override void ExecuteTransaction(decimal amount)
        {
            Console.WriteLine("Write + for a deposit and - for a withdrawl");
            string Transaction = Console.ReadLine();

            if (Transaction == "+")
            {


                Balance += amount;
                Console.WriteLine("Transaction successful");
            }

            if (Transaction == "-")
            {
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine("Transaction successful");
                }
                else
                {
                    Console.WriteLine("Transaction unsuccessful");
                }
            }
        }
        public override void PrintTransaction()
        {
            Console.WriteLine("Write + for a deposit and - for a withdrawl");
            string Transaction = Console.ReadLine();

            if (Transaction == "+")
            {
                Console.WriteLine("How much amount has to be deposited?");
                string amount_string = Console.ReadLine();
                decimal amount = decimal.Parse(amount_string);

                Balance += amount;
                Console.WriteLine($"DEPOSIT of {amount} successful. New Balance: {Balance}");
            }

            if (Transaction == "-")
            {
                Console.WriteLine("How much amount has to be deducted?");
                string amount_string = Console.ReadLine();
                decimal amount = decimal.Parse(amount_string);
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine($"WITHDRAWAL of {amount} successful. New Balance: {Balance}");
                }
                else
                {
                    Console.WriteLine($"Insufficient funds... Total Balance = {Balance}");
                }
            }
        }

        public override void CalculateInterest()
        {
            decimal interest_rate = 0.000m;
            Console.WriteLine($"Interest for this Checking accounts is {interest_rate * 100}");
        }
    }
    class LoanAccount : BankAccount, ITransaction
    {
        //A new vriable is being introduced that is privy only to Loan Accounts. Loans can be granted based
        //on either credit score or amount of value stored inside the account. For simplicity, we are going to
        //use user's credit score to check the maximum value of loan they can avail.

        decimal Loan_credit_score;
        public LoanAccount(int account_number, string account_holder_Name, decimal balance, decimal loan_credit)
            : base(account_holder_Name, account_number, balance)
        {
            Loan_credit_score = loan_credit;
        }

        public override void ExecuteTransaction(decimal amount)
        {
            Console.WriteLine("Write + for a deposit and - for a withdrawl");
            string Transaction = Console.ReadLine();

            if (Transaction == "+")
            {


                Balance += amount;
                Console.WriteLine("Transaction successful");
            }

            if (Transaction == "-")
            {
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine("Transaction successful");
                }
                else
                {
                    Console.WriteLine("Transaction unsuccessful");
                }
            }
        }
        public override void PrintTransaction()
        {
            Console.WriteLine("Write + for a deposit and - for a withdrawl");
            string Transaction = Console.ReadLine();

            if (Transaction == "+")
            {
                Console.WriteLine("How much amount has to be deposited?");
                string amount_string = Console.ReadLine();
                decimal amount = decimal.Parse(amount_string);

                Balance += amount;
                Console.WriteLine($"DEPOSIT of {amount} successful. New Balance: {Balance}");
            }

            if (Transaction == "-")
            {
                Console.WriteLine("How much amount has to be deducted?");
                string amount_string = Console.ReadLine();
                decimal amount = decimal.Parse(amount_string);
                if (Balance >= amount)
                {
                    Balance -= amount;
                    Console.WriteLine($"WITHDRAWAL of {amount} successful. New Balance: {Balance}");
                }
                else
                {
                    Console.WriteLine($"Insufficient funds... Total Balance = {Balance}");
                }
            }
        }

        public override void CalculateInterest()
        {
            decimal interest_rate = 0.000m;
            Console.WriteLine($"Interest rate for Loan accounts is {interest_rate * 100}%");
        }

        public void GrantLoan()
        {

            decimal Max_Loan = Loan_credit_score * 10000;

            Console.WriteLine("Enter Loan demanded");
            string loan = Console.ReadLine();
            decimal loan_int = decimal.Parse(loan);

            if (loan_int <= Max_Loan)
            {
                Console.WriteLine("Loan granted");
            }
            else
            {
                Console.WriteLine("Loan limit exceeded");
            }
        }
    }
    class Bank
    {

        //We will now be start dealing "BankAccount" as a datatype in a greater frequency. Just like we
        //find ourselves specifying datatypes inside parenthesis of signatures, we will now see 
        //.Add command, List command and "BankAccount" being specified as a data type inside the parenthesis.
        private List<BankAccount> all_accounts;
        public Bank()
        {
            all_accounts = new List<BankAccount>();
        }

        public void AddAccount(BankAccount account)
        {
            all_accounts.Add(account);
            Console.WriteLine("The Bank account has been added into the database");
        }

        public void DepositToAccount(int account_number, int amount)
        {
            if (account_number != 235200)
            {
                BankAccount account = Find_my_account(account_number);
                if (account == null)
                {
                    Console.WriteLine("No account exists against given account number");
                }
                else
                {
                    account.Deposit(amount);
                }
            }

            else
            {
                SavingsAccount account = (SavingsAccount)Find_my_account(account_number);
                if (account == null)
                {
                    Console.WriteLine("No account exists against given account number");
                }
                else
                {
                    account.Deposit(amount);
                }
            }
        }



        public void WithdrawFromAccount(int accountNumber, int amount)
        {
            BankAccount account = Find_my_account(accountNumber);
            if (account != null)
            {
                account.Withdraw(amount);
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }

        public void GrantLoan()
        {
            Console.WriteLine("Enter Client's account number");
            string acc_no = Console.ReadLine();
            int acc_no_int = int.Parse(acc_no);

            //Explicit cast was used to demonstrate that a Loan Account data type can also be used for this method.

            LoanAccount account = (LoanAccount)Find_my_account(acc_no_int);

            if (account != null)
            {
                account.GrantLoan();
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }
        private BankAccount Find_my_account(int account_number)
        {
            foreach (BankAccount account in all_accounts)
            {
                if (account.Account_number == account_number)
                {
                    return account;
                }
            }
            return null;
        }
    }
}