using System.Collections.Generic;

namespace WebApplication3.Models

{
    public class TransactionType
    {
        public int TransactionTypeId { get; set; }
        public string Name { get; set; } = "";
        public List<Transaction> Transactions { get; set; } = new();
    }
}
    
