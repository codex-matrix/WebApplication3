using System;

namespace WebApplication3.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }

        public int ClientId { get; set; }
        public Client? Client { get; set; }

        public int TransactionTypeId { get; set; }
        public TransactionType? TransactionType { get; set; }

        public decimal Amount { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}

