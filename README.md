
# Banking Management System

This code represents a simple banking system implemented in C#. It consists of several classes that model transactions and a bank.

The `Transaction` class is an abstract base class that provides common functionality for different types of transactions. It has fields to store the transaction amount, success status, execution status, reversal status, and a date stamp. It also provides virtual methods for success, execution, reversal, printing, execution, and rollback.

The `Bank` class represents a bank and has a list of transactions and accounts. It provides methods to add transactions and accounts to the bank, retrieve an account by name, execute a transaction, rollback a transaction, and print the transaction history.

The `Account` class represents a bank account and has fields for balance and account name. It provides methods to deposit, withdraw, and print the account details.

There are three derived transaction classes: `WithdrawTransaction`, `DepositTransaction`, and `TransferTransaction`. Each of these classes inherits from the `Transaction` base class and overrides the necessary methods. They also contain specific logic for executing and rolling back the corresponding transaction types.

The `BankSystem` class contains the main entry point `Main` method, which interacts with the user through a console-based menu. It provides options to withdraw, deposit, transfer, print account details, add an account, find an account, print transaction history, perform a rollback, and quit the program. It uses the `Bank` class to perform these operations.

Overall, this code implements a basic banking system with support for different types of transactions, account management, and transaction history tracking.

## Deployment

To deploy this code, follow the steps below:

    1. Clone the repository to your local machine.
    2. Open the solution file in Visual Studio or any   other C# IDE of your choice.
    3. Build the solution to ensure that all the necessary dependencies are installed.
    4. Run the application to test its functionality.
    5. Use the provided classes to implement your banking system.

Note:

    1. Ensure that you have the .NET Framework installed on your machine.
## Developer
- [@kanishk0263](https://github.com/kanishkjain0263)
