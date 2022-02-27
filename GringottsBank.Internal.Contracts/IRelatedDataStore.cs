using System.Threading.Tasks;

namespace GringottsBank.Internal.Contracts
{
    public interface IRelatedDataStore<I, O>
    {
        Task<O> Read(I input);
    }
}
