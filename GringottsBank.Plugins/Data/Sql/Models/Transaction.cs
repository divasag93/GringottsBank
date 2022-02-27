using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace GringottsBank.Plugins.Data.Sql
{
    [Table("Transaction")]
    public partial class Transaction
    {
        [Key]
        [StringLength(50)]
        public string Id { get; set; }
        public float AccountNumber { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerId { get; set; }
        [Required]
        [StringLength(50)]
        public string TxnType { get; set; }
        public float Amount { get; set; }
        public float ClosingBalance { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(AccountNumber))]
        [InverseProperty(nameof(Account.Transactions))]
        public virtual Account AccountNumberNavigation { get; set; }
        [ForeignKey(nameof(CustomerId))]
        [InverseProperty("Transactions")]
        public virtual Customer Customer { get; set; }
    }
}
