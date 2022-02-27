namespace GringottsBank.Internal.Contracts
{
    public interface IFilter<T> {
        bool IsSatisfied(T item);
    }
}
