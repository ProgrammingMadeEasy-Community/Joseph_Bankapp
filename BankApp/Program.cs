using System;
using System.Collections.Generic;

namespace BankApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to BankApp!");

            // Initialize a new bank account
            BankAccount account = new BankAccount();

            while (true)
            {
                Console.WriteLine("\nMenu:");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Set Transaction PIN");
                Console.WriteLine("4. Make Transfer");
                Console.WriteLine("5. Make Withdrawal");
                Console.WriteLine("6. View Account Details");
                Console.WriteLine("7. View Transaction History");
                Console.WriteLine("8. Exit");
                Console.Write("Please select an option: ");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            Console.Write("Enter Account Name: ");
                            string name = Console.ReadLine();
                            account.CreateAccount(name);
                            Console.WriteLine($"Account created for {account.AccountName} with Account Number {account.AccountNumber}");
                            break;

                        case 2:
                            Console.Write("Enter deposit amount: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal depositAmount))
                            {
                                account.Deposit(depositAmount);
                                Console.WriteLine($"Deposited {depositAmount:C}. New Balance: {account.Balance:C}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount entered.");
                            }
                            break;

                        case 3:
                            Console.Write("Enter new Transaction PIN: ");
                            string newPin = Console.ReadLine();
                            account.SetTransactionPIN(newPin);
                            Console.WriteLine("Transaction PIN set successfully.");
                            break;

                        case 4:
                            Console.Write("Enter recipient's account number: ");
                            string recipientAccountNumber = Console.ReadLine();
                            Console.Write("Enter transfer amount: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal transferAmount))
                            {
                                Console.Write("Enter your Transaction PIN: ");
                                string pinAttempt = Console.ReadLine();
                                if (account.ValidatePIN(pinAttempt))
                                {
                                    if (account.MakeTransfer(recipientAccountNumber, transferAmount))
                                    {
                                        Console.WriteLine($"Transfer successful. New Balance: {account.Balance:C}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Transfer failed. Check recipient's account number.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Transaction PIN.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount entered.");
                            }
                            break;

                        case 5:
                            Console.Write("Enter withdrawal amount: ");
                            if (decimal.TryParse(Console.ReadLine(), out decimal withdrawalAmount))
                            {
                                Console.Write("Enter your Transaction PIN: ");
                                string pinAttempt = Console.ReadLine();
                                if (account.ValidatePIN(pinAttempt))
                                {
                                    if (account.MakeWithdrawal(withdrawalAmount))
                                    {
                                        Console.WriteLine($"Withdrawal successful. New Balance: {account.Balance:C}");
                                    }
                                    else
                                    {
                                        Console.WriteLine("Insufficient balance.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid Transaction PIN.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid amount entered.");
                            }
                            break;

                        case 6:
                            Console.WriteLine($"Account Name: {account.AccountName}");
                            Console.WriteLine($"Account Number: {account.AccountNumber}");
                            Console.WriteLine($"Balance: {account.Balance:C}");
                            break;

                        case 7:
                            Console.WriteLine("Transaction History:");
                            account.DisplayTransactionHistory();
                            break;

                        case 8:
                            Console.WriteLine("Exiting BankApp. Have a nice day!");
                            return;

                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a valid number.");
                }
            }
        }
    }

    class BankAccount
    {
        public string AccountName { get; private set; }
        public string AccountNumber { get; private set; }
        public decimal Balance { get; private set; }
        private string TransactionPIN { get; set; }
        private List<string> TransactionHistory { get; set; } = new List<string>();
        private int WrongPINAttempts { get; set; } = 0;
        private bool IsLocked { get; set; } = false;


        public void CreateAccount(string accountName)
        {
            AccountName = accountName;
            AccountNumber = GenerateAccountNumber();
            Balance = 0;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount;
            LogTransaction($"Deposit: +{amount:C}");
        }

        public void SetTransactionPIN(string pin)
        {
            TransactionPIN = pin;
        }

        public bool ValidatePIN(string pinAttempt)
        {
            if (IsLocked)
            {
                Console.WriteLine("Account is locked. Contact customer support.");
                return false;
            }

            if (pinAttempt == TransactionPIN)
            {
                WrongPINAttempts = 0; // Reset wrong attempts
                return true;
            }
            else
            {
                WrongPINAttempts++;
                Console.WriteLine($"Invalid PIN. Attempts remaining: {3 - WrongPINAttempts}");
                if (WrongPINAttempts >= 3)
                {
                    Console.WriteLine("Account locked. Contact customer support.");
                    IsLocked = true;
                }
                return false;
            }
        }

        public bool MakeTransfer(string recipientAccountNumber, decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                LogTransaction($"Transfer to {recipientAccountNumber}: -{amount:C}");
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool MakeWithdrawal(decimal amount)
        {
            if (Balance >= amount)
            {
                Balance -= amount;
                LogTransaction($"Withdrawal: -{amount:C}");
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DisplayTransactionHistory()
        {
            foreach (var transaction in TransactionHistory)
            {
                Console.WriteLine(transaction);
            }
        }

        private string GenerateAccountNumber()
        {
            // In a real system, you would generate a unique account number
            // For simplicity, we'll use a random 6-digit number here.
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private void LogTransaction(string transaction)
        {
            TransactionHistory.Add($"{DateTime.Now}: {transaction}");
        }
    }
}
