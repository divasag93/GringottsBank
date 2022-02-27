using System;

namespace GringottsBank.Internal.DataStore.Contracts
{
    public partial class Account
    {
        public float Number { get; set; }
        public string CustomerId { get; set; }
        public float CurrentBalance { get; set; }
        public DateTime OpeningDateTime { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
