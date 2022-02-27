using System.Collections.Generic;
using System.Linq;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Plugins.Data.Sql
{
    public static class Translator
    {
        public static Customer TranslateToDbCustomer(DataStore.Customer customer, Customer dbCustomer = null, ICollection<Account> accounts = null, ICollection<Transaction> transactions = null)
        {
            if (dbCustomer == null)
                dbCustomer = new Customer();

            dbCustomer.Id = customer.Id;
            dbCustomer.FirstName = customer.FirstName;
            dbCustomer.MiddleName = customer.MiddleName;
            dbCustomer.LastName = customer.LastName;
            dbCustomer.DateOfBirth = customer.DateOfBirth;
            dbCustomer.Gender = customer.Gender;
            dbCustomer.ProofName = customer.ProofName;
            dbCustomer.Proof = customer.Proof;
            dbCustomer.Accounts = accounts;
            dbCustomer.Transactions = transactions;

            return dbCustomer;
        }

        public static DataStore.Customer TranslateCustomerToDataSource(Customer dbCustomer, DataStore.Customer customer)
        {
            if (customer == null)
                customer = new DataStore.Customer();

            customer.Id = dbCustomer.Id;
            customer.FirstName = dbCustomer.FirstName;
            customer.MiddleName = dbCustomer.MiddleName;
            customer.LastName = dbCustomer.LastName;
            customer.DateOfBirth = dbCustomer.DateOfBirth;
            customer.ProofName = dbCustomer.ProofName;
            customer.Proof = dbCustomer.Proof;
            return customer;
        }

        public static DataStore.Account TranslateAccountToDataSource(Account dbAccount, DataStore.Account account)
        {
            if (account == null)
                account = new DataStore.Account();

            account.CustomerId = dbAccount.CustomerId;
            if(account.Customer == null)
                account.Customer = TranslateCustomerToDataSource(dbAccount.Customer, account.Customer);
            account.CurrentBalance = dbAccount.CurrentBalance;
            account.Number = dbAccount.Number;
            account.OpeningDateTime = dbAccount.OpeningDateTime;

            return account;
        }

        public static DataStore.Transaction TranslateTransactionToDataSource(Transaction dbTransaction)
        {
            return new DataStore.Transaction
            {
                DateTime = dbTransaction.DateTime,
                AccountNumber = dbTransaction.AccountNumber,
                Amount = dbTransaction.Amount,
                ClosingBalance = dbTransaction.ClosingBalance,
                CustomerId = dbTransaction.CustomerId,
                Id = dbTransaction.Id,
                TxnType = dbTransaction.TxnType
            };
        }

        public static Account TranslateToDbAccount(DataStore.Account account, Account dbAccount, Customer dbCustomer, ICollection<Transaction> transactions = null)
        {
            if(dbAccount == null)
               dbAccount = new Account();

            dbAccount.CustomerId = account.CustomerId;
            dbAccount.Customer = dbCustomer;
            if(dbCustomer.Accounts.FirstOrDefault(account => account.Number == dbAccount.Number) == null)
            {
                dbCustomer.Accounts.Add(dbAccount);
            }
            dbAccount.CurrentBalance = account.CurrentBalance;
            dbAccount.Number = account.Number;
            dbAccount.OpeningDateTime = account.OpeningDateTime;
            dbAccount.Transactions = transactions;

            return dbAccount;
        }

        public static Transaction TranslateToDbTransaction(DataStore.Transaction transaction, Transaction dbTransaction, Account dbAccount, Customer dbCustomer)
        {
            if (dbTransaction == null)
                dbTransaction = new Transaction();

            dbTransaction.Id = transaction.Id;
            dbTransaction.TxnType = transaction.TxnType;
            dbTransaction.DateTime = transaction.DateTime;
            dbTransaction.CustomerId = transaction.CustomerId;
            dbTransaction.Customer = dbCustomer;
            dbTransaction.ClosingBalance = transaction.ClosingBalance;
            dbTransaction.Amount = transaction.Amount;
            dbTransaction.AccountNumberNavigation = dbAccount;
            dbTransaction.AccountNumber = transaction.AccountNumber;

            return dbTransaction;
        }
    }
}
