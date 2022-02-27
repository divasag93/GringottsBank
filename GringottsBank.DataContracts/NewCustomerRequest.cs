

namespace GringottsBank.DataContracts
{
    public class NewCustomerRequest
    {
        public Name CustomerName { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
        public IdentityProof ProofOfIdentity { get; set; }
    }

    public class NewAccountRequest
    {
        public string CustomerId { get; set; }
        public float InitialDeposit { get; set; }
    }

    public class TransactionRequest
    {
        public string AccountNumber { get; set; }
        public float Amount { get; set; }
    }

    public class TransactionDetailsRequest
    {
        public string AccountNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
