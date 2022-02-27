using System.Collections.Generic;
using System.Threading.Tasks;
using GringottsBank.DataContracts;

namespace GringottsBank.Internal.Contracts
{
    public interface ITransactionInfoService
    {
        Task<IEnumerable<Transaction>> FetchTransactionsAsync(TransactionDetailsRequest transactionDetailsRequest);
    }
}
