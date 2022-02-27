using System;
using System.Collections.Generic;

namespace GringottsBank.Internal.DataStore.Contracts
{
    public class Customer
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string ProofName { get; set; }
        public string Proof { get; set; }
    }
}
