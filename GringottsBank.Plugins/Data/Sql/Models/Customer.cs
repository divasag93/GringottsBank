using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace GringottsBank.Plugins.Data.Sql
{
    [Table("Customer")]
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(50)]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [StringLength(10)]
        public string Gender { get; set; }
        [Required]
        [StringLength(50)]
        public string ProofName { get; set; }
        [Required]
        public string Proof { get; set; }

        [InverseProperty(nameof(Account.Customer))]
        public virtual ICollection<Account> Accounts { get; set; }
        [InverseProperty(nameof(Transaction.Customer))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
