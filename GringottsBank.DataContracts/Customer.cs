using System;

namespace GringottsBank.DataContracts
{
    public class Customer
    {
        public string Id { get; set; }
        public Name Name { get; set; }
        public Gender Gender { get; set; }
        public string DateOfBirth { get; set; }
    }
}
