using GringottsBank.DataContracts;
using System;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{
    public static class Translator
    {
        #region Customer
        public static DataStore.Customer TranslateToDataSource(NewCustomerRequest newCustomerRequest)
        {
            return new DataStore.Customer
            {
                Id = GenerateCustomerId(newCustomerRequest.ProofOfIdentity.Proof, newCustomerRequest.CustomerName.First, newCustomerRequest.CustomerName.Last),
                DateOfBirth = System.DateTime.Parse(newCustomerRequest.DateOfBirth),
                FirstName = newCustomerRequest.CustomerName.First,
                MiddleName = newCustomerRequest.CustomerName.Middle,
                LastName = newCustomerRequest.CustomerName.Last,
                Gender = newCustomerRequest.Gender.ToString(),
                ProofName = newCustomerRequest.ProofOfIdentity.Name,
                Proof = newCustomerRequest.ProofOfIdentity.Proof
            };
        }

        internal static DataStore.Session TranslateToDataSource(LoginRequest loginRequest)
        {
            return new DataStore.Session
            {
                CustomerId = loginRequest.CustomerId
            };
        }

        public static Customer TranslateToDataContract(DataStore.Customer customer)
        {
            return new Customer
            {
                Id = customer.Id,
                DateOfBirth = customer.DateOfBirth.ToString("yyyy-MM-dd"),
                Gender = Enum.Parse<Gender>(customer.Gender),
                Name = new Name
                {
                    First = customer.FirstName,
                    Middle = customer.MiddleName,
                    Last = customer.LastName
                }
            };
        }

        private static string GenerateCustomerId(string proof, string firstName, string lastName)
        {
            return string.Format("{0}{1}{2}", proof, firstName.Substring(0, 3), lastName.Substring(0, 3));
        }

        #endregion

        #region Account
        public static DataStore.Account TranslateToDataSource(NewAccountRequest newAccountRequest, DataStore.Customer customer)
        {
            return new DataStore.Account
            {
                CustomerId = newAccountRequest.CustomerId,
                CurrentBalance = newAccountRequest.InitialDeposit,
                Customer = customer,
                Number = GenerateAccountNumber(),
                OpeningDateTime = DateTime.UtcNow
            };
        }

        internal static DataStore.Account TranslateToDataSource(DataStore.Account accountInDs, float newBalance)
        {
            accountInDs.CurrentBalance = newBalance;
            return accountInDs;
        }

        internal static DataStore.Transaction TranslateToDataSource(DataStore.Account accountInDs, float amount, float closingBalance, bool isWithdraw = true)
        {
            return new DataStore.Transaction
            {
                AccountNumber = accountInDs.Number,
                Amount = amount,
                ClosingBalance = closingBalance,
                CustomerId = accountInDs.CustomerId,
                DateTime = DateTime.UtcNow,
                Id = Guid.NewGuid().ToString(),
                TxnType = isWithdraw ? "Withdraw" : "Deposit"
            };
        }

        internal static Transaction TranslateToDataContract(DataStore.Transaction transactionInDs)
        {
            return new Transaction
            {
                DateTime = transactionInDs.DateTime.ToString("yyyy-MM-dd"),
                Amount = transactionInDs.Amount,
                ClosingBalance = transactionInDs.ClosingBalance,
                Id = transactionInDs.Id,
                Type = transactionInDs.TxnType
            };
        }

        public static DataContracts.Account TranslateToDataContract(DataStore.Account account)
        {
            return new Account
            {
                AccountNumber = account.Number.ToString(),
                CurrentBalance = account.CurrentBalance,
                CustomerId = account.CustomerId
            };
        }

        private static float GenerateAccountNumber()
        {
            Random random = new Random();
            int accNo = random.Next(10000000, 20000000);
            return float.Parse(accNo.ToString());
        }

        #endregion
    }
}
