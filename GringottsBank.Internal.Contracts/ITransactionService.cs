using GringottsBank.DataContracts;
using System.Threading.Tasks;

namespace GringottsBank.Internal.Contracts
{
    public interface ITransactionService
    {
        Task<Transaction> Withdraw(string accountNumber, float amount);
        Task<Transaction> Deposit(string accountNumber, float amount);
    }
}
