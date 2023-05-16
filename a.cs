public abstract class Transaction
{
    double _amount;
    public bool _success;
    public bool _executed;
    public bool _reversed;
    public DateTime _dateStamp = DateTime.Now;

    public virtual bool Success()
    {
        return this._success;
    }

    public virtual bool Executed()
    {
        return this._executed;
    }

    public virtual bool Reversed()
    {
        return this._reversed;
    }

    public DateTime DateStamp()
    {
        return this._dateStamp;
    }

    public Transaction(double amount) //Constructor
    {
        this._amount = amount;
    }

    public virtual void Print()
    {
        System.Console.WriteLine("The transaction amount is: " + _amount);
    }

    public virtual void Execute()
    {
        System.Console.WriteLine("The transaction us under execution...");
        
    }

    public virtual void Rollback()
    {
        System.Console.WriteLine("ROLLBACK is under execution....");
    }
}

public class Bank
{
    public List<Transaction> _transactions = new List<Transaction>();

    public void AddTransaction(Transaction transaction)
    {
        this._transactions.Add(transaction);
    }

    public List<Account> Accounts_List = new List<Account>();

    public void AddAccount(Account account)
    {
        Accounts_List.Add(account);
    }

    public void GetAccount(Account accountName)
    {
        bool is_Account_In_List = Accounts_List.Contains(accountName);
        if (is_Account_In_List)
            System.Console.WriteLine(Accounts_List.Contains(accountName));
        else
            System.Console.WriteLine("Null");
    }

    public void ExecuteTransaction(Transaction trans)
    {
        trans.Execute();
        AddTransaction(trans);
    }

    public void RollbackTransaction(Transaction trans)
    {
        trans.Rollback();
    }

    public void PrintTransactionHistory()
    {
        //foreach (Transaction transaction in _transactions)
        //{
        //    System.Console.WriteLine("----------------TRANSACTION HISTORY---------------");
        //    if (transaction.Success())
        //    {
        //        Console.ForegroundColor = ConsoleColor.Green;
        //    }
        //    else if (transaction.Reversed())
        //    {
        //        Console.ForegroundColor = ConsoleColor.Red;
        //    }
        //    transaction.Print();
        //    Console.ForegroundColor = ConsoleColor.White;
        //}
        System.Console.WriteLine(_transactions.Count);
        for(int i = 0; i < _transactions.Count; i++)
        {
            _transactions[i].Print();
        }
    }

    public class Account
    {
        //Instance Variables
        public double _balance;
        public string _name;

        //Constructor
        public Account(double balance, string name)
        {
            this._balance = balance;
            this._name = name;
        }

        public bool Deposit(double depositAmount)
        {
            Boolean TEMP = false;
            if (depositAmount > 0)
            {
                this._balance += depositAmount;
                //Console.WriteLine("The total balance after deposit is: " + this._balance);
                TEMP = true;
            }
            else
            {
                throw new InvalidOperationException("Please enter positive amount to deposit!");
            }
            return TEMP;
        }

        public bool Withdraw(double withdrawAmount)
        {
            bool temp = false;
            if (withdrawAmount > 0 && _balance >= withdrawAmount)
            {
                this._balance -= withdrawAmount;
                //Console.WriteLine("The total balance after withdrawl is: " + this._balance);
                temp = true;
            }
            else
            {
                throw new InvalidOperationException("Please enter a valid amount to withdraw.");
            }
            return temp;
        }

        public void Print()
        {
            Console.WriteLine(
                $"The name of the account holder is {_name} \n The balance of the account is {_balance}"
            );
        }

        public String Name()
        {
            return _name;
        }

        public double Balance()
        {
            return _balance;
        }
    }

    class WithdrawTransaction : Transaction
    {
        private Account _account;
        private double _amount;

        public WithdrawTransaction(Account account, double amount) : base(amount)
        {
            this._account = account;
            this._amount = amount;
        }

        public override void Print()
        {
            if(Reversed()) System.Console.WriteLine("Deposit Rollbacked  from acccount: "+_account._name + "\n Amount: " + _account._balance);
            else System.Console.WriteLine("Deposit performed from acccount: "+_account._name + "\n Amount: " + _account._balance);
        }

        public override void Execute()
        {
            if (this._account.Withdraw(_amount))
            {
                this._success = true;
                _executed = true;
                _reversed = false;
            }
            else
            {
                this._success = false;
                _executed = false;
                _reversed = true;

                try { }
                catch (InvalidOperationException)
                {
                    throw new InvalidOperationException(
                        "The withdrawl operation is unseccessfull due to the following reasons:\n1. The transaction is already attempted.\n2. The amount to withdraw is more than the balance available in the account."
                    );
                }
            }
        }

        public override void Rollback()
        {
            if (_reversed == false && this._success == true)
            {
                _account.Deposit(_amount);
                _reversed = true;
            }
            else if (_reversed == true)
            {
                System.Console.WriteLine("Rollback already done.");
            }
        }

        public override Boolean Executed()
        {
            return _executed;
        }

        public override Boolean Success()
        {
            return _success;
        }

        public override Boolean Reversed()
        {
            return _reversed;
        }
    }

    class DepositTransaction : Transaction
    {
        private Account _account;
        private double _amount;

        public DepositTransaction(Account account, double amount) : base(amount)
        {
            this._account = account;
            this._amount = amount;
        }

        public override void Print()
        {
            if(Reversed()) System.Console.WriteLine("Deposit Rollbacked  from acccount: "+_account._name + "\n Amount: " + _account._balance);
            else System.Console.WriteLine("Deposit performed from acccount: "+_account._name + "\n Amount: " + _account._balance);
        }

        public override void Execute()
        {
            if (this._account.Deposit(_amount))
            {
                this._success = true;
                _executed = true;
                _reversed = false;
            }
            else
            {
                this._success = false;
                _executed = false;
                _reversed = true;

                throw new InvalidOperationException(
                    "The deposit operation is unseccessfull due to the following reasons:\n1. The transaction is already attempted.\n2. The amount to withdraw is more than the balance available in the account."
                );
            }
        }

        public override void Rollback()
        {
            if (_reversed == false && this._success == true)
            {
                _account.Withdraw(_amount);
                _reversed = true;
            }
            else if (_reversed == true)
            {
                throw new InvalidOperationException(
                    "The Rollback operation is unseccessfull due to the following reasons:\n1. The transaction is already reversed.\n2. The original transaction is not yet finalized."
                );
            }
        }

        public override Boolean Executed()
        {
            return _executed;
        }

        public override Boolean Success()
        {
            return _success;
        }

        public override Boolean Reversed()
        {
            return _reversed;
        }
    }

    class TransferTransaction : Transaction
    {
        private Account _fromAccount;
        private Account _toAccount;
        private double _amount;
        private DepositTransaction _deposit;
        private WithdrawTransaction _withdraw;

        public TransferTransaction(Account fromAccount, Account toAccount, double amount) : base(amount)
        {
            this._amount = amount;
            this._fromAccount = fromAccount;
            this._toAccount = toAccount;
            this._withdraw = new WithdrawTransaction(fromAccount, amount);
            this._deposit = new DepositTransaction(toAccount, amount);
        }

        public override void Print()
        {
            _withdraw.Print();
            _deposit.Print();
            System.Console.WriteLine(
                $"Successfully transferred ${this._amount} from {this._fromAccount} {this._fromAccount._name}'s account to {this._toAccount} {this._toAccount._name}'s account."
            );
        }

        public override void Execute()
        {
            if (_executed == true)
            {
                throw new InvalidOperationException(
                    "The transfer transaction has failed due to following reasons:\n1. The transaction has already been executed. \n2. The account to be debited doesn't have sufficient funds. \n3. The deposit has failed."
                );
            }
            else if (this._toAccount == this._fromAccount)
            {
                throw new InvalidOperationException(
                    "Error: The transfer account cannot be the same!!"
                );
            }
            else
            {
                _withdraw.Execute();
                _deposit.Execute();
                this._executed = true;
            }
        }

        public override void Rollback()
        {
            if (_reversed == false)
            {
                _withdraw.Rollback();
                _deposit.Rollback();

                _reversed = true;
            }
            else if (_reversed)
            {
                throw new InvalidOperationException(
                    "The operation has failed because of one or more of the following reasons: \n1. The original transaction has not been completed. \n2. Transaction has already been reversed. \n3. The account which has to be debited has insufficient balance."
                );
            }
        }

        public override Boolean Executed()
        {
            return _executed;
        }

        public override Boolean Reversed()
        {
            return _reversed;
        }
    }

    public enum MenuOption
    {
        Withdraw,
        Deposit,
        Transfer,
        Print,
        AddAccount,
        FindAccount,
        TransactionHistory,
        DoRollback,
        Quit
    }

    class BankSystem
    {
        static MenuOption ReadUserOption()
        {
            int option;

            do
            {
                Console.WriteLine(
                    "1. Withdraw \n2. Deposit \n3. Transfer \n4. Print \n5. Add New Account \n6. Find Account \n7. Print Transaction History \n8. Perform Rollback \n9. Quit "
                );
                Console.Write("Please Enter An Option Number: ");

                option = Convert.ToInt32(Console.ReadLine());
                option--;
                if (option > -1 && option < 9 || option == 9)
                {
                    break;
                }

                if (option < 0 || option > 9)
                {
                    Console.WriteLine(
                        "Error: Please enter a option number that matches the menu!!"
                    );
                    Console.WriteLine();
                }

                // option--;
            } while (true);

            MenuOption option1 = (MenuOption)option;
            return option1;
        }

        static void DoWithdraw(Bank bank)
        {
            Console.WriteLine("Please enter the amount to be withdrawled in the account: ");
            int amount = Convert.ToInt32(Console.ReadLine());
            Account new_acc = FindAccount(bank);
            WithdrawTransaction withdraw = new WithdrawTransaction(new_acc, amount);
            bank.ExecuteTransaction(withdraw);
            Console.WriteLine("Do you want to ROLLBACK the transaction? (Y/N)");
            string yes_no = Console.ReadLine();
            if (yes_no.ToLower() == "y")
            {
                bank.RollbackTransaction(withdraw);
            }
            withdraw.Print();
        }

        static void DoDeposit(Bank bank)
        {
            Console.WriteLine("Please entert the amount to be deposited in the account: ");
            int amount = Convert.ToInt32(Console.ReadLine());
            Account new_acc = FindAccount(bank);
            DepositTransaction deposit = new DepositTransaction(new_acc, amount);
            bank.ExecuteTransaction(deposit);
            System.Console.WriteLine("Do you want to ROLLBACK the transaction? (Y/N)");
            string yes_no = Console.ReadLine();
            if (yes_no.ToLower() == "y")
            {
                bank.RollbackTransaction(deposit);
            }
            deposit.Print();
        }

        static void DoTransfer(Bank bank)
        {
            try
            {
                Console.WriteLine("Please enter the amount to be transferred from the account: ");
                int amount = Convert.ToInt32(Console.ReadLine());
                Account fromAccount = FindAccount(bank);
                Account toAccount = FindAccount(bank);
                TransferTransaction transfer = new TransferTransaction(
                    fromAccount,
                    toAccount,
                    amount
                );
                bank.ExecuteTransaction(transfer);
                System.Console.WriteLine("Do you want to ROLLBACK the transaction? (Y/N)");
                string yes_no = Console.ReadLine();
                if (yes_no.ToLower() == "y")
                {
                    bank.RollbackTransaction(transfer);
                }
                transfer.Print();
            }
            catch (InvalidOperationException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        static void DoPrint(Bank bank)
        {
            foreach (Account acc in bank.Accounts_List)
            {
                acc.Print();
            }
        }

        static void AddAccount(Bank bank)
        {
            System.Console.WriteLine();
            System.Console.Write("Please enter the name of the account: ");
            String Account_name = Console.ReadLine();
            System.Console.WriteLine();
            System.Console.Write("Enter the Account Starting Balance: ");
            int Account_balance = Convert.ToInt32(Console.ReadLine());

            Account temp = new Account(Account_balance, Account_name);
            bank.AddAccount(temp);
        }

        public List<Account> Accounts_List = new List<Account>();

        static Account FindAccount(Bank bank)
        {
            System.Console.WriteLine("Please enter the accout name to search: ");
            string name = Console.ReadLine();

            foreach (Account account in bank.Accounts_List)
            {
                if (account._name.ToLower() == name.ToLower())
                {
                    return account;
                }
            }
            return null;
        }

        static void Print_Transaction_History(Bank bank)
        {
            bank.PrintTransactionHistory();
        }

        static void DoRollBack(Bank bank)
        {
            System.Console.WriteLine(
                "Enter the index of transaction you want to perform rollback on:"
            );
            int index = Convert.ToInt32(Console.ReadLine());
            if (index < bank._transactions.Count && index >= 0)
            {
                bank.RollbackTransaction(bank._transactions[index]);
            }
            else
            {
                throw new InvalidOperationException("Error: Invalid transaction attempt!");
            }
        }

        static void Main(String[] args)
        {
            Account Account_1 = new Account(5000, "Kanishk");
            Account Account_2 = new Account(10000, "Khushi");
            Bank bank = new Bank();
            bank.AddAccount(Account_1);
            bank.AddAccount(Account_2);
            do
            {
                MenuOption option_Select = ReadUserOption();

                switch (option_Select)
                {
                    case MenuOption.Withdraw:
                        DoWithdraw(bank);
                        break;

                    case MenuOption.Deposit:
                        DoDeposit(bank);
                        break;

                    case MenuOption.Transfer:
                        DoTransfer(bank);
                        break;

                    case MenuOption.Print:
                        DoPrint(bank);
                        break;

                    case MenuOption.AddAccount:
                        AddAccount(bank);
                        break;

                    case MenuOption.FindAccount:
                        FindAccount(bank);
                        break;

                    case MenuOption.TransactionHistory:
                        Print_Transaction_History(bank);
                        break;

                    case MenuOption.DoRollback:
                        DoRollBack(bank);
                        break;

                    case MenuOption.Quit:
                        System.Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine();
                        break;
                }
            } while (true);
        }
    }
}
