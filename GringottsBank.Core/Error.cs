namespace GringottsBank.Core
{
    public static class Error
    {
        public static class Code
        {
            public static readonly string CustomerAlreadyExists = "1";
            public static readonly string CustomerDoesNotExists = "2";
            public static readonly string AccountCreationError = "3";
            public static readonly string AccountDoesNotExist = "4";
            public static readonly string InSufficientBalance = "5";
            public static readonly string TransactionFailed = "6";
            public static readonly string InvalidTransactionAmount = "7";
            public static readonly string ApplicationException = "8";
            public static readonly string CustomerIdNotProvided = "9";
            public static readonly string FirstNameRequired = "10";
            public static readonly string LastNameRequired = "11";
            public static readonly string NameRequired = "12";
            public static readonly string InvalidDOB = "13";
            public static readonly string AgeConstraint = "14";
            public static readonly string MissingCustomerID = "15";
            public static readonly string MissingAccountNumber = "16";
            public static readonly string InvalidAccountNumber = "17";
            public static readonly string MissingDateOfBirth = "18";
            public static readonly string MissingIdentityProof = "19";
            public static readonly string InvalidFromDate = "20";
            public static readonly string InvalidToDate = "21";
            public static readonly string NotWholeNumber = "22";
        }

        public static class Message
        {
            public static readonly string CustomerAlreadyExists = "Customer with this identity already exists.";
            public static readonly string CustomerDoesNotExists = "Customer with this identity does not exist.";
            public static readonly string AccountCreationError = "An error occured while creating the account, please try again.";
            public static readonly string AccountDoesNotExist = "Account with this number does not exists.";
            public static readonly string InSufficientBalance = "Insufficient balance. Cannot process.";
            public static readonly string TransactionFailed = "Transaction failed. Please try again.";
            public static readonly string InvalidTransactionAmount = "Invalid transaction amount.";
            public static readonly string ApplicationException = "An application exception occured.";
            public static readonly string CustomerIdNotProvided = "Customer id is required.";
            public static readonly string FirstNameRequired = "Customer first name is required.";
            public static readonly string LastNameRequired = "Customer last name is required.";
            public static readonly string NameRequired = "Customer first & last name are required.";
            public static readonly string InvalidDOB = "Invalid value passed as dateOfBirth.";
            public static readonly string AgeConstraint = "You must be atleast 15 years old to become our customer.";
            public static readonly string MissingCustomerID = "Customer id is required.";
            public static readonly string MissingAccountNumber = "Account number is required.";
            public static readonly string InvalidAccountNumber = "Account number provided is invalid.";
            public static readonly string MissingDateOfBirth = "DateOfBirth is required.";
            public static readonly string MissingIdentityProof = "IdentityProof is required.";
            public static readonly string InvalidFromDate = "Invalid value provided for from date.";
            public static readonly string InvalidToDate = "Invalid value provided for to date.";
            public static string NotWholeNumber(string property) => string.Format("Invalid value provided for {0}.", property);
        }
    }
}
