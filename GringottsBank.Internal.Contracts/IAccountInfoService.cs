using System.Collections.Generic;
using System.Threading.Tasks;
using GringottsBank.DataContracts;

namespace GringottsBank.Internal.Contracts
{
    public interface IAccountInfoService
    {
        Task<Account> CreateAsync(NewAccountRequest newAccountRequest);
        Task<Account> GetAsync(string accountNumber);
        Task<IEnumerable<Account>> GetAllAsync(string customerId);
    }
}
