using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Plugins.Data.Sql
{

    public class SqlAccountTransactionsDataSource : IDataStore<DataStore.Transaction>,
                                                    IRelatedDataStore<DataStore.Account, List<DataStore.Transaction>>
    {
        public SqlAccountTransactionsDataSource(GringottsBankContext sqlDbContext, 
            IDataStore<DataStore.Customer> customerDataSource, IDataStore<DataStore.Account> accountDataSource)
        {
            _sqlDbContext = sqlDbContext;
            _customerDataSource = customerDataSource;
            _accountDataSource = accountDataSource;
            
        }

        private readonly GringottsBankContext _sqlDbContext;
        private readonly IDataStore<DataStore.Customer> _customerDataSource;
        private readonly IDataStore<DataStore.Account> _accountDataSource;

        public async Task<DataStore.Transaction> Create(DataStore.Transaction input)
        {
            var accountInDb = _sqlDbContext.Accounts.FirstOrDefault(account =>  account.Number == input.AccountNumber);
            var customerInDb = _sqlDbContext.Customers.FirstOrDefault(customer => customer.Id == input.CustomerId);
            var txn = Translator.TranslateToDbTransaction(input, null, accountInDb, customerInDb);
            _sqlDbContext.Transactions.Add(txn);

            try
            {
                await _sqlDbContext.SaveWithRetryAsync();
            }
            catch(Exception ex)
            {

            }

            return input;
        }

        public async Task<DataStore.Transaction> Read(DataStore.Transaction input)
        {
            var matchingTxn = _sqlDbContext.Transactions.FirstOrDefault(txn => txn.Id == input.Id);
            if(matchingTxn == null)
            {
                return null;
            }
            return Translator.TranslateTransactionToDataSource(matchingTxn);
        }

        public Task<DataStore.Transaction> Update(DataStore.Transaction input)
        {
            throw new NotImplementedException();
        }

        public async Task<DataStore.Transaction> Delete(DataStore.Transaction input)
        {
            var matchingTxn = _sqlDbContext.Transactions.FirstOrDefault(txn => txn.Id == input.Id);
            if (matchingTxn == null)
            {
                return null;
            }
            _sqlDbContext.Transactions.Remove(matchingTxn);
            try
            {
                await _sqlDbContext.SaveWithRetryAsync();
            }
            catch (Exception ex)
            {

            }
            return Translator.TranslateTransactionToDataSource(matchingTxn);
        }

        public async Task<List<DataStore.Transaction>> Read(DataStore.Account input)
        {
            var matchingTransactions = _sqlDbContext.Transactions.Where(txn => txn.AccountNumber == input.Number);
            if(matchingTransactions == null || matchingTransactions.Count() == 0)
            {
                return null;
            }
            var txns = new List<DataStore.Transaction>();
            matchingTransactions.ToList().ForEach(txn => txns.Add(Translator.TranslateTransactionToDataSource(txn)));
            return txns;
        }
    }
}
