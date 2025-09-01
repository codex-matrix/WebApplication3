using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Models;

namespace WebApplication3.Services
{
    public class AddTransactionResult
    {
        public Transaction Transaction { get; set; } = null!;
        public decimal NewBalance { get; set; }
    }

    public class TransactionService
    {
        private readonly AppDbContext _db;
        public TransactionService(AppDbContext db) { _db = db; }

        public async Task<AddTransactionResult> AddTransactionAsync(int clientId, int transactionTypeId, decimal amount, string? comment)
        {
          
            await using var tran = await _db.Database.BeginTransactionAsync();

            var client = await _db.Clients.FindAsync(clientId)
                         ?? throw new InvalidOperationException("Client not found");

            var txType = await _db.TransactionTypes.FindAsync(transactionTypeId)
                         ?? throw new InvalidOperationException("Transaction type not found");

            var tx = new Transaction
            {
                ClientId = clientId,
                TransactionTypeId = transactionTypeId,
                Amount = amount,
                Comment = comment,
                CreatedAt = DateTime.UtcNow
            };

            _db.Transactions.Add(tx);

        
            if (txType.Name.Equals("Debit", StringComparison.OrdinalIgnoreCase))
                client.Balance -= amount;
            else
                client.Balance += amount;

            await _db.SaveChangesAsync();
            await tran.CommitAsync();

           
            var savedTx = await _db.Transactions
                                   .Include(t => t.TransactionType)
                                   .FirstOrDefaultAsync(t => t.TransactionId == tx.TransactionId)
                                   ?? tx;

            return new AddTransactionResult { Transaction = savedTx, NewBalance = client.Balance };
        }

        public async Task UpdateTransactionCommentAsync(int transactionId, string? comment)
        {
            var tx = await _db.Transactions.FindAsync(transactionId)
                     ?? throw new InvalidOperationException("Transaction not found");
            tx.Comment = comment;
            await _db.SaveChangesAsync();
        }

        public async Task<List<Transaction>> GetTransactionsForClientAsync(int clientId)
        {
            return await _db.Transactions
                .Where(t => t.ClientId == clientId)
                .Include(t => t.TransactionType)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}
