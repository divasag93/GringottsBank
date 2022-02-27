using System;

namespace GringottsBank.Internal.DataStore.Contracts
{
    public partial class Transaction
    {
        public string Id { get; set; }
        public float AccountNumber { get; set; }
        public string CustomerId { get; set; }
        public string TxnType { get; set; }
        public float Amount { get; set; }
        public float ClosingBalance { get; set; }
        public DateTime DateTime { get; set; }
    }
}
