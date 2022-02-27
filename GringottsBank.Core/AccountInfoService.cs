using System.Collections.Generic;
using System.Threading.Tasks;
using GringottsBank.Core.Validators;
using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{

    public class AccountInfoService : IAccountInfoService
    {
        private readonly IDataStore<DataStore.Account> _accountDataStore;
        private readonly IDataStore<DataStore.Customer> _customerDataStore;
        private readonly IRelatedDataStore<DataStore.Customer, List<DataStore.Account>> _customerAccountsRelatedDataStore;
        public AccountInfoService(IDataStore<DataStore.Account> accountDataStore,
                                  IDataStore<DataStore.Customer> customerDataStore,
                                  IRelatedDataStore<DataStore.Customer, List<DataStore.Account>> customerAccountsRelatedDataStore)
        {
            _accountDataStore = accountDataStore;
            _customerDataStore = customerDataStore;
            _customerAccountsRelatedDataStore = customerAccountsRelatedDataStore;
        }
        public async Task<Account> CreateAsync(NewAccountRequest newAccountRequest)
        {
            var validator = new NewAccountRequestValidator();
            var validationResult = validator.Validate(newAccountRequest);
            if(validationResult.IsValid == false)
            {
                var errors = new List<DataContracts.Error>();
                validationResult.Errors.ForEach(error => errors.Add(new DataContracts.Error { Code = error.ErrorCode, Message = error.ErrorMessage }));
                Failure.BadRequest(errors);
            }

            var customer = await _customerDataStore.Read(new DataStore.Customer { Id = newAccountRequest.CustomerId });
           
            if (customer == null)
                Failure.BadRequest(Error.Code.CustomerDoesNotExists, Error.Message.CustomerDoesNotExists);

            var accountOpenReq = Translator.TranslateToDataSource(newAccountRequest, customer);
            var accountInDs = await _accountDataStore.Read(new DataStore.Account { Number = accountOpenReq.Number });

            while (accountInDs != null)
            {
                accountOpenReq = Translator.TranslateToDataSource(newAccountRequest, customer);
                accountInDs = await _accountDataStore.Read(new DataStore.Account { Number = accountOpenReq.Number });
            }

            accountInDs = await _accountDataStore.Create(accountOpenReq);

            if (accountInDs == null)
                Failure.BadRequest(Error.Code.AccountCreationError, Error.Message.AccountCreationError);

            return Translator.TranslateToDataContract(accountInDs);
        }

        public async Task<IEnumerable<Account>> GetAllAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                Failure.BadRequest(Error.Code.MissingCustomerID, Error.Message.MissingCustomerID);

            var customerInDs = await _customerDataStore.Read(new DataStore.Customer { Id = customerId });
            if (customerInDs == null)
                Failure.BadRequest(Error.Code.CustomerDoesNotExists, Error.Message.CustomerDoesNotExists);

            var accounts = await _customerAccountsRelatedDataStore.Read(customerInDs);
            var customerAccounts = new List<Account>();
            if (accounts == null)
                return customerAccounts;
            accounts.ForEach(account => customerAccounts.Add(Translator.TranslateToDataContract(account)));
            return customerAccounts;
        }

        public async Task<Account> GetAsync(string accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
                Failure.BadRequest(Error.Code.MissingAccountNumber, Error.Message.MissingAccountNumber);

            var accountInDs = await _accountDataStore.Read(new DataStore.Account { Number = float.Parse(accountNumber) });
            if (accountInDs == null)
                Failure.BadRequest(Error.Code.AccountDoesNotExist, Error.Message.AccountDoesNotExist);

            return Translator.TranslateToDataContract(accountInDs);
        }
    }
}
