using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Task.Data.DAL;
using Task.Data.Entities;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<Category> categories = _context.Categories.Where(categories =>
                                      categories.IsDeleted == false).ToList();
            return StatusCode(200, categories);
        }

        [HttpGet("{isdelete}")]
        public IActionResult GetDeleted()
        {
            List<Category> categories = _context.Categories.Where(categories =>
                                      categories.IsDeleted).ToList();
            return StatusCode(200, categories);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Category category = _context.Categories.Where(category =>
                                      category.IsDeleted == false).FirstOrDefault(c => c.Id == id);

            if (category == null) return NotFound();

            return StatusCode(200, category);
            //return Ok products
        }


        [HttpPost("")]
        public IActionResult Create(Category category)
        {
            bool isExistName = _context.Categories.Any(c=>c.Name==category.Name);

            if (isExistName)
            {
                return BadRequest("Already exist");
            }

            Category newCategory = new Category();
            newCategory.Name = category.Name;
            newCategory.Description=category.Description;
            _context.Add(newCategory);
            _context.SaveChanges();

            return StatusCode(201, newCategory);
        }


        [HttpPut("")]
        public IActionResult Update(int?id,Category category)
        {
            if (id==null)
            {
                return NotFound();
            }
            Category dbcategory = _context.Categories.Where(c => c.IsDeleted == false)
                .FirstOrDefault(c => c.Id == id);

            if (dbcategory == null)
            {
                return NotFound();
            }

            Category dbcategoryWithName  = _context.Categories.Where(c => c.IsDeleted == false)
               .FirstOrDefault(c => c.Name==category.Name);


            if (dbcategory!=null)
            {
                if (dbcategory!=dbcategoryWithName)
                {
                    return BadRequest("Already exist");
                }
            }

            dbcategory.Name = category.Name;
            dbcategory.Description = category.Description;
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            Category category = _context.Categories.Where(category =>
                                      category.IsDeleted == false).FirstOrDefault(c=>c.Id==id);

            if (category == null) return NotFound();
            category.IsDeleted = true;
            _context.SaveChanges();

            return StatusCode(200, category);
            //return Ok products
        }
    }
}
