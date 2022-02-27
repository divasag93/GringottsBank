using System.Collections.Generic;
using System.Threading.Tasks;
using GringottsBank.Core.Validators;
using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;
using DataStore = GringottsBank.Internal.DataStore.Contracts;

namespace GringottsBank.Core
{
    public class CustomerDataServices : ICustomerService
    {
        private readonly IDataStore<DataStore.Customer> _customerDataStore;
        public CustomerDataServices(IDataStore<DataStore.Customer> customerDataStore)
        {
            _customerDataStore = customerDataStore;
        }
        public async Task<Customer> CreateAsync(NewCustomerRequest newCustomer)
        {
            var validator = new NewCustomerRequestValidator();
            var validationResult = validator.Validate(newCustomer);
            if(validationResult.IsValid == false)
            {
                var errors = new List<DataContracts.Error>();
                validationResult.Errors.ForEach(error => errors.Add(new DataContracts.Error { Code = error.ErrorCode, Message = error.ErrorMessage }));
                throw new BadRequestException(errors);
            }

            var data = Translator.TranslateToDataSource(newCustomer);
            var customer = await _customerDataStore.Read(new DataStore.Customer { Id = data.Id });

            if (customer != null)
                Failure.BadRequest(Error.Code.CustomerAlreadyExists, Error.Message.CustomerAlreadyExists);

            var customerInDS = await _customerDataStore.Create(data);
            return Translator.TranslateToDataContract(customerInDS);
        }

        public async Task<Customer> GetAsync(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
                Failure.BadRequest(Error.Code.MissingCustomerID, Error.Message.MissingCustomerID);

            var customerInDS = await _customerDataStore.Read(new DataStore.Customer {Id = customerId });
            if (customerInDS == null)
                Failure.BadRequest(Error.Code.CustomerDoesNotExists, Error.Message.CustomerDoesNotExists);
            return Translator.TranslateToDataContract(customerInDS);
        }
    }
}
