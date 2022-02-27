using System.Threading.Tasks;

namespace GringottsBank.Internal.Contracts
{
    public interface IDataStore<T>
    {
        Task<T> Create(T input);
        Task<T> Read(T input);
        Task<T> Update(T input);
        Task<T> Delete(T input);
    }
}
