using GringottsBank.DataContracts;
using System.Threading.Tasks;

namespace GringottsBank.Internal.Contracts
{
    public interface ISessionManager
    {
        Task<string> NewSession(LoginRequest loginRequest);
        Task<string> GetCustomerSessionData(string token);
        Task<bool> IsSessionActive(string token);
    }
}
