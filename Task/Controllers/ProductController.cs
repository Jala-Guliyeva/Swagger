using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task.Data.DAL;
using Task.Data.Entities;
using Task.Dto;
using Task.Extention;

namespace Task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _env = env;
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            //List<Product> products = _context.Products.Include(products=>products.Category).Where(products =>
            //                          products.IsDeleted == false).ToList();

           List<ProductReturnDto> productList = _context.Products.Select(p => new ProductReturnDto
            {
                Name = p.Name,
                Price=p.Price,
                Description=p.Description,
                CategoryName=p.Category.Name,
                ImageUrl= $"https://localhost:44367/image/{p.ImageUrl}"
            }).ToList();
            
            
            return StatusCode(200, productList);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            Product product = _context.Products.Include(products => products.Category).Where(products =>
                                  products.IsDeleted == false).FirstOrDefault(product=>product.Id==id);
            if (product==null)
            {
                return NotFound();
            }

            product.ImageUrl = $"https://localhost:44367/image/{product.ImageUrl}";
            return StatusCode(200, product);
        }
       

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm]ProductCreateDto productCreateDto)
        {
            Product newProduct = new Product();

            if (!productCreateDto.Image.IsImage())
            {
                return BadRequest("shekil secin");
            }

            if (productCreateDto.Image.CheckSize(20000))
            {
                return BadRequest("olcu boyukdur");
            }

            newProduct.Name = productCreateDto.Name;
            newProduct.Description = productCreateDto.Description;
            newProduct.CategoryId = productCreateDto.CategoryId;
            newProduct.Price = productCreateDto.Price;
            newProduct.ImageUrl =await productCreateDto.Image.SaveImage(_env,"image");
            _context.Add(newProduct);
            _context.SaveChanges();

            return StatusCode(201, newProduct);
        }
    }
}
