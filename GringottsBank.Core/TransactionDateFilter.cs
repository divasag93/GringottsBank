using System;
using GringottsBank.DataContracts;
using GringottsBank.Internal.Contracts;

namespace GringottsBank.Core
{
    public class TransactionDateFilter : IFilter<Transaction>
    {
        public TransactionDateFilter(string from, string to)
        {
            if (string.IsNullOrEmpty(from))
                from = "1976-01-01";
            if (string.IsNullOrEmpty(to))
                to = "2076-01-01";
            _from = DateTime.Parse(from);
            _to = DateTime.Parse(to);
        }
        private readonly DateTime _from, _to;
        public bool IsSatisfied(Transaction item)
        {
            var txnDate = DateTime.Parse(item.DateTime);
            var fromDateComparision = DateTime.Compare(_from, txnDate);
            var toDateComparision = DateTime.Compare(txnDate, _to);
            return (fromDateComparision < 0 && toDateComparision < 0) || fromDateComparision == 0 || toDateComparision == 0;
        }
    }
}
