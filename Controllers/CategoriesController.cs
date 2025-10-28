using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using WebApplication1.Data;
using WebApplication1.DTO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly FirstAPIContext _context;
        private readonly IMapper _mapper;
        private object categoryDto;

        public CategoriesController(FirstAPIContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // get categories for specific book
        [HttpGet("books/{bookId}")]
        public async Task<IActionResult> GetCategories(int bookId)
        {
            var catelogiesForSpecificBook = await _context.Categories
                 .Where(c => c.BookId == bookId)
                 .Include(c => c.Book)
                 .ToListAsync();

            return Ok(catelogiesForSpecificBook);
        }

        // get category by id
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid category ID.");
            }

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult> getCategories()
        {
            var categories = await _context.Categories.ToListAsync();

            //_mapper.Map(categoryDto, category);

            return Ok(categories);
        }


        [HttpPost]
        public async Task<ActionResult<Category>> AddCategory(CategoryDto categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);

            if (string.IsNullOrWhiteSpace(category.Name))
            {

                return BadRequest("Category name is required");
            }

            var bookExist = await _context.Books.AnyAsync(b => b.Id == category.Id);

            //if (!bookExist)
            //{
            //    return BadRequest("Invalid bookId - book doesnot exist");
            //}

            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
        {
            var categoryEntity = await _context.Categories.FindAsync(id);

            if (categoryEntity == null)
            {
                return NotFound($"Category with id {id} not found");

            }


            categoryEntity.Name = categoryDto.Name;

            //_mapper.Map(category, categoryDto);

            await _context.SaveChangesAsync();

            return Ok(categoryEntity);
        }


        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.Include(c => c.Transactions).SingleOrDefaultAsync(c => c.Id == id);

            if (category == null) {
                return NotFound();
            }

            if (category.Transactions != null && category.Transactions.Any())
            {
                return BadRequest("Cannot delete a category that has transaction");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(id);
        }
       


    }

    }
