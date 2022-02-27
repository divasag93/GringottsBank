namespace GringottsBank.DataContracts
{
    public class Transaction
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public double ClosingBalance { get; set; }
        public string DateTime { get; set; }
    }
}
