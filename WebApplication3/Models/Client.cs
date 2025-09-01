
using System;
using System.Collections.Generic;

namespace WebApplication3.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string? Email { get; set; }
        public decimal Balance { get; set; }

        public List<Transaction> Transactions { get; set; } = new();
    }
}