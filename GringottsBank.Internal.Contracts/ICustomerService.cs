using GringottsBank.DataContracts;
using System.Threading.Tasks;

namespace GringottsBank.Internal.Contracts
{
    public interface ICustomerService
    {
        Task<Customer> CreateAsync(NewCustomerRequest newCustomer);
        Task<Customer> GetAsync(string customerId);
    }

    public interface ICustomerAuthorizationService
    {
        Task<bool> IsAuthorizedCustomer(string customerId, string password);
        Task<bool> IsAccountAccessable(string accountNumber, string customerId);
    }
}
