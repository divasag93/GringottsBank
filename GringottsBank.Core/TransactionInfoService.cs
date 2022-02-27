using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GringottsBank.Core.Validators;
using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;
using DataSource = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{
    public class TransactionInfoService : ITransactionInfoService
    {
        private readonly IDataStore<DataSource.Account> _accountDataSource;
        private readonly IRelatedDataStore<DataSource.Account, List<DataSource.Transaction>> _accountTransactionsRelatedDataStore;

        public TransactionInfoService(IDataStore<DataSource.Account> accountDataSource,
                                      IRelatedDataStore<DataSource.Account, List<DataSource.Transaction>> accountTransactionsRelatedDataStore)
        {
            _accountDataSource = accountDataSource;
            _accountTransactionsRelatedDataStore = accountTransactionsRelatedDataStore;
        }
        public async Task<IEnumerable<Transaction>> FetchTransactionsAsync(TransactionDetailsRequest transactionDetailsRequest)
        {
            var validationResult = (new TransactionDetailsRequestValidator()).Validate(transactionDetailsRequest);
            if(validationResult.IsValid == false)
            {
                var errors = new List<DataContracts.Error>();
                validationResult.Errors.ForEach(error => errors.Add(new DataContracts.Error { Code = error.ErrorCode, Message = error.ErrorMessage }));
                Failure.BadRequest(errors);
            }

            var accountInDs = await _accountDataSource.Read(new DataSource.Account { Number = float.Parse(transactionDetailsRequest.AccountNumber) });
            if (accountInDs == null)
                throw new BadRequestException(new DataContracts.Error
                {
                    Code = Error.Code.AccountDoesNotExist,
                    Message = Error.Message.AccountDoesNotExist
                });

            var txns = await _accountTransactionsRelatedDataStore.Read(Translator.TranslateToDataSource(accountInDs, accountInDs.CurrentBalance));
            List<Transaction> transactions = new List<Transaction>();
            txns.ForEach(txn => transactions.Add(Translator.TranslateToDataContract(txn)));
            IFilter<Transaction> _transactionFilter = new TransactionDateFilter(transactionDetailsRequest.From, transactionDetailsRequest.To);
            transactions = transactions.Where(txn => _transactionFilter.IsSatisfied(txn)).ToList();
            return transactions;
        }
    }
}
