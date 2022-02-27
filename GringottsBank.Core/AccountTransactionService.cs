using System.Threading.Tasks;
using GringottsBank.Core.Validators;
using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{
    public class AccountTransactionService : ITransactionService
    {
        private readonly IDataStore<DataStore.Transaction> _transactionDataStore;
        private readonly IDataStore<DataStore.Account> _accountDataStore;
        public AccountTransactionService(IDataStore<DataStore.Transaction> transactionDataStore, IDataStore<DataStore.Account> accountDataStore)
        {
            _transactionDataStore = transactionDataStore;
            _accountDataStore = accountDataStore;
        }
        public async Task<Transaction> Deposit(string accountNumber, float amount)
        {
            ValidateTxnRequest(accountNumber, amount);

            var accountInDs = await FetchAccountFromDataStore(accountNumber);

            var closingBalance = accountInDs.CurrentBalance + amount;

            accountInDs = await _accountDataStore.Update(Translator.TranslateToDataSource(accountInDs, closingBalance));
            if (accountInDs == null)
                Failure.BadRequest(Error.Code.TransactionFailed, Error.Message.TransactionFailed);

            var transactionInDs = await _transactionDataStore.Create(Translator.TranslateToDataSource(accountInDs, amount, closingBalance, false));
            return Translator.TranslateToDataContract(transactionInDs);
        }

        public async Task<Transaction> Withdraw(string accountNumber, float amount)
        {
            ValidateTxnRequest(accountNumber, amount);

            var accountInDs = await FetchAccountFromDataStore(accountNumber);

            if (accountInDs.CurrentBalance - amount < 0)
                Failure.BadRequest(Error.Code.InSufficientBalance, Error.Message.InSufficientBalance);

            var closingBalance = accountInDs.CurrentBalance - amount;
            accountInDs = await _accountDataStore.Update(Translator.TranslateToDataSource(accountInDs, closingBalance));

            if (accountInDs == null)
                Failure.BadRequest(Error.Code.TransactionFailed, Error.Message.TransactionFailed);

            var transactionInDs = await _transactionDataStore.Create(Translator.TranslateToDataSource(accountInDs, amount, closingBalance));
            return Translator.TranslateToDataContract(transactionInDs);
        }

        private void ValidateTxnRequest(string accountNumber, float amount)
        {
            if (string.IsNullOrEmpty(accountNumber))
                Failure.BadRequest(Error.Code.MissingAccountNumber, Error.Message.MissingAccountNumber);

            if ((new WholeNumberValidator("accountNumber")).Validate(accountNumber).IsValid == false)
                Failure.BadRequest(Error.Code.InvalidAccountNumber, Error.Message.InvalidAccountNumber);

            if (amount <= 0)
                Failure.BadRequest(Error.Code.InvalidTransactionAmount, Error.Message.InvalidTransactionAmount);
        }

        private async Task<DataStore.Account> FetchAccountFromDataStore(string accountNumber)
        {
            var accountInDs = await _accountDataStore.Read(new DataStore.Account { Number = float.Parse(accountNumber) });
            if (accountInDs == null)
                Failure.BadRequest(Error.Code.AccountDoesNotExist, Error.Message.AccountDoesNotExist);

            return accountInDs;
        }
    }
}
