using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace GringottsBank.Plugins.Data.Sql
{
    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

        [Key]
        public float Number { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerId { get; set; }
        public float CurrentBalance { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime OpeningDateTime { get; set; }

        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Accounts")]
        public virtual Customer Customer { get; set; }
        [InverseProperty(nameof(Transaction.AccountNumberNavigation))]
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
