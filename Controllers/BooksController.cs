using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net.NetworkInformation;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {

        private readonly FirstAPIContext _context;
        private readonly IMapper _mapper; 
        public BooksController(FirstAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<List<Book>>> GetBooks()
         
        {
            var books = await _context.Books
         .Include(b => b.User)
         .Include(b => b.Categories)
         .ToListAsync();

            return Ok(books);
        }


        [HttpGet("{id}")]
        public async Task <ActionResult<Book>> GetBookById(int id)
        {
            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(book);
            }
        }

        [HttpPost]
        public async Task< ActionResult<Book>> AddBook(BookDto bookDto )
        {
            var newBook = _mapper.Map<Book>(bookDto);

            if (newBook == null)
            {
                return BadRequest();
            }
            else
            {
                _context.Books.Add(newBook);
                await _context.SaveChangesAsync();
                //books.Add(newBook);
                return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Book>> EditBook(int id, BookDto bookDto)
        {

            var book = await _context.Books.FindAsync(id);

            if (book == null)

                return NotFound();

            _mapper.Map(bookDto,book);

            
            await _context.SaveChangesAsync();


            return NoContent();

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {


            var book = await _context.Books.FindAsync(id);

            if (book == null)
            {
                return NotFound();

            }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return NoContent();

        }
    };
}
