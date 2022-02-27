using System;
using System.Linq;
using System.Threading.Tasks;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Plugins.Data.Sql
{
    public class SqlCustomerDataSource : IDataStore<DataStore.Customer>
    {
        public SqlCustomerDataSource(GringottsBankContext sqlDbContext)
        {
            _sqlDbContext = sqlDbContext;
        }

        private readonly GringottsBankContext _sqlDbContext;

        public async Task<DataStore.Customer> Create(DataStore.Customer input)
        {
            var dbCustomer = Translator.TranslateToDbCustomer(input);
            _sqlDbContext.Customers.Add(dbCustomer);
            try
            {
                await _sqlDbContext.SaveWithRetryAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return input;
        }

        public async Task<DataStore.Customer> Read(DataStore.Customer input)
        {
            var dbCustomer = _sqlDbContext.Customers.FirstOrDefault(customer => customer.Id == input.Id);
            if(dbCustomer == null)
            {
                return null;
            }
            return Translator.TranslateCustomerToDataSource(dbCustomer, input);
        }

        public async Task<DataStore.Customer> Update(DataStore.Customer input)
        {
            var dbCustomer = _sqlDbContext.Customers.FirstOrDefault(customer => customer.Id == input.Id);
            if(dbCustomer == null)
            {

            }
            dbCustomer = Translator.TranslateToDbCustomer(input, dbCustomer, dbCustomer.Accounts, dbCustomer.Transactions);
            try
            {
                await _sqlDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            return input;
        }

        public async Task<DataStore.Customer> Delete(DataStore.Customer input)
        {
            var dbCustomer = _sqlDbContext.Customers.FirstOrDefault(customer => customer.Id == input.Id);
            if(dbCustomer == null)
            {

            }
            _sqlDbContext.Customers.Remove(dbCustomer);

            try
            {
                await _sqlDbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {

            }
            return input;
        }
    }
}
