using System.Linq;
using System.Threading.Tasks;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{
    public class CustomerAuthorizationService : ICustomerAuthorizationService
    {

        private readonly IDataStore<DataStore.Credentials> _customerCredsDataStore;
        private readonly IAccountInfoService _accountInfoService;
        private readonly IDataStore<DataStore.Customer> _customerDataStore;

        public CustomerAuthorizationService(IDataStore<DataStore.Customer> customerDataStore,
                                            IDataStore<DataStore.Credentials> customerCredsDataStore,
                                            IAccountInfoService accountInfoService)
        {
            _customerDataStore = customerDataStore;
            _customerCredsDataStore = customerCredsDataStore;
            _accountInfoService = accountInfoService;
        }

        public async Task<bool> IsAccountAccessable(string accountNumber, string customerId)
        {
            if (string.IsNullOrEmpty(accountNumber))
                Failure.BadRequest(Error.Code.MissingAccountNumber, Error.Message.MissingAccountNumber);

            var customerAccounts = await _accountInfoService.GetAllAsync(customerId);

            if (customerAccounts == null || customerAccounts.Count() == 0)
            {
                return false;
            }

            var accounts = customerAccounts.ToList();

            return accounts.Exists(account => account.AccountNumber == accountNumber);
        }

        public async Task<bool> IsAuthorizedCustomer(string customerId, string password)
        {
            if (string.IsNullOrEmpty(customerId))
                Failure.BadRequest(Error.Code.MissingCustomerID, Error.Message.MissingCustomerID);
            if (string.IsNullOrEmpty(password)) { }

            var customerInDS = await _customerDataStore.Read(new DataStore.Customer { Id = customerId });
            if (customerInDS == null)
                Failure.BadRequest(Error.Code.CustomerDoesNotExists, Error.Message.CustomerDoesNotExists);

            var credsInDs = _customerCredsDataStore.Read(new DataStore.Credentials { CustomerId = customerId, Password = password });
            if (credsInDs == null)
                return false;
            return true;
        }
    }
}
