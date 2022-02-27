using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Plugins.Data.Sql
{
    public class SqlAccountsDataSource : IDataStore<DataStore.Account>,
                                         IRelatedDataStore<DataStore.Customer, List<DataStore.Account>>
    {
        public SqlAccountsDataSource(GringottsBankContext sqlDbContext, IDataStore<DataStore.Customer> customerDataSource)
        {
            _sqlDbContext = sqlDbContext;
            _customerDataSource = customerDataSource;
        }

        private readonly GringottsBankContext _sqlDbContext;
        private readonly IDataStore<DataStore.Customer> _customerDataSource;

        public async Task<DataStore.Account> Create(DataStore.Account input)
        {
            var dbCustomer = _sqlDbContext.Customers.FirstOrDefault(customer => customer.Id == input.CustomerId);
            var dbAccount = Translator.TranslateToDbAccount(input, null, dbCustomer);
            _sqlDbContext.Accounts.Add(dbAccount);
            try
            {
                await _sqlDbContext.SaveWithRetryAsync();
            }
            catch (Exception ex)
            {

            }
            return Translator.TranslateAccountToDataSource(dbAccount, input);
        }

        public async Task<DataStore.Account> Read(DataStore.Account input)
        {
            var account = _sqlDbContext.Accounts.FirstOrDefault(account => account.Number == input.Number);
            if (account == null)
                return null;
            account.Customer = _sqlDbContext.Customers.FirstOrDefault(c => c.Id == account.CustomerId);
            return Translator.TranslateAccountToDataSource(account, input);
        }

        public async Task<DataStore.Account> Update(DataStore.Account input)
        {
            var dbCustomer = _sqlDbContext.Customers.FirstOrDefault(customer => customer.Id == input.CustomerId);
            var dbAccount = dbCustomer.Accounts.FirstOrDefault(account => account.Number == input.Number);
            dbAccount = Translator.TranslateToDbAccount(input, dbAccount, dbCustomer, dbAccount.Transactions);
            try
            {
                await _sqlDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return null;
            }
            return Translator.TranslateAccountToDataSource(dbAccount, input);
        }

        public async Task<DataStore.Account> Delete(DataStore.Account input)
        {
            var dbAccount = _sqlDbContext.Accounts.FirstOrDefault(account => account.Number == input.Number);
            if (dbAccount == null)
            {
                return null;
            }
            _sqlDbContext.Accounts.Remove(dbAccount);
            try
            {
                await _sqlDbContext.SaveWithRetryAsync();
            }
            catch (Exception ex)
            {

            }
            return input;
        }

        public async Task<List<DataStore.Account>> Read(DataStore.Customer input)
        {
            var customerId = input.Id;
            var relatedAccounts = _sqlDbContext.Accounts.Where(account => account.CustomerId == customerId);
            if (relatedAccounts == null || relatedAccounts.Count() == 0)
            {
                return null;
            }
            var accounts = new List<DataStore.Account>();
            var accs = relatedAccounts.ToList();
            accs.ForEach(account =>
            {
                accounts.Add(Translator.TranslateAccountToDataSource(account, null));
            });

            return accounts;
        }
    }
}
