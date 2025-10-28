using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Transactions;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase

    {

        private readonly FirstAPIContext _context;
        private readonly IMapper _mapper;
        public TransactionsController(FirstAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult> getAllTransactions()
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .OrderByDescending(t => t.Date)
                .ToListAsync();
            return Ok(transactions);
        }

        [HttpGet("by-category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<Models.Transaction>>> GetTransactionByBook(int categoryId)
        {
            var transactions = await _context.Transactions
           .Where(t => t.CategoryId == categoryId)
           .Include(t => t.Category)
           .OrderByDescending(t => t.Date)
           .ToListAsync();

            return Ok(transactions);
        }



        [HttpGet("by-bookId/{bookId}")]

        public async Task<ActionResult> getTransactionByBook(int bookId)
        {
            var transactions = await _context.Transactions
                .Include(t => t.Category)
                .Where(t => t.Category.BookId == bookId)
                .ToListAsync();

            if(transactions.Count == 0 || transactions == null)
            {
                return NotFound($"No transactionFound for BookId {bookId}");
            }

            return Ok(transactions);
        }



        [HttpPost]
        public async Task<ActionResult<Models.Transaction>> AddTransaction(TransactionDto transactionDto)
        {
            var transaction = _mapper.Map<Models.Transaction>(transactionDto);

            if (transaction.Amount == 0)
            {
                return BadRequest("Amount must be greater than 0");
            }

            if (transaction.Type != "CashIn" && transaction.Type != "CashOut")
            {
                return BadRequest("Type Must be 'CashIn' or 'CashOut' ");
            }

            if (transaction.Date == default)
            {
                transaction.Date = DateTime.UtcNow;

            }

            _context.Transactions.Add(transaction);

            await _context.SaveChangesAsync();

            return Ok(transaction);
        }


        [HttpPut("{id}")]
        public async  Task<ActionResult<Models.Transaction>> EditTransaction(int id,TransactionDto transactionDto)
        {
            var existingTransaction = await _context.Transactions.FindAsync(id);

            if (existingTransaction == null)
            {
                return NotFound($"Transaction with Id = {id} not found");
            }

            _mapper.Map(transactionDto, existingTransaction);

            //_context.Entry(existingTransaction).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(existingTransaction);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Models.Transaction>> DeleteTranaction (int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            Console.WriteLine("trans", transaction);

            if (transaction == null)
                return NotFound("Transaction Not Found");

             _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted successfully" });

        }


        [HttpGet("summary/Book/{bookId}")]

        public async Task<ActionResult<object>> GetBookSummary (int bookId)
        {


            var transactions = await _context.Transactions
        .Include(t => t.Category)
        .Where(t => t.Category != null && t.Category.BookId == bookId)
        .ToListAsync();

            var totalIn = transactions.Where(t => t.Type == "CashIn").Sum(t => t.Amount);
            var totalOut = transactions.Where(t => t.Type == "CashOut").Sum(t => t.Amount);
            var balance = totalIn - totalOut;

            return Ok(new
            {
                TotalCashIn = totalIn,
                TotalCashOut = totalOut,
                Balance = balance
            });

        }

        

    }
}
