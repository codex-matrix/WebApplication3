using Microsoft.AspNetCore.Mvc;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly TransactionService _svc;
        public TransactionsController(TransactionService svc) { _svc = svc; }

        [HttpGet("ByClient/{clientId}")]
        public async Task<IActionResult> ByClient(int clientId)
        {
            var txs = await _svc.GetTransactionsForClientAsync(clientId);
           
            var result = txs.Select(t => new {
                t.TransactionId,
                t.ClientId,
                TransactionType = t.TransactionType?.Name,
                t.Amount,
                t.Comment,
                CreatedAt = t.CreatedAt
            });
            return Ok(result);
        }

        public class AddTransactionDto
        {
            public int ClientId { get; set; }
            public int TransactionTypeId { get; set; }
            public decimal Amount { get; set; }
            public string? Comment { get; set; }
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddTransactionDto dto)
        {
            var res = await _svc.AddTransactionAsync(dto.ClientId, dto.TransactionTypeId, dto.Amount, dto.Comment);
            return Ok(new { TransactionId = res.Transaction.TransactionId, NewBalance = res.NewBalance });
        }

        public class UpdateCommentDto
        {
            public int TransactionId { get; set; }
            public string? Comment { get; set; }
        }

        [HttpPost("UpdateComment")]
        public async Task<IActionResult> UpdateComment([FromBody] UpdateCommentDto dto)
        {
            await _svc.UpdateTransactionCommentAsync(dto.TransactionId, dto.Comment);
            return Ok();
        }
    }
}