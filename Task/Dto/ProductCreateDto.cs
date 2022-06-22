using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Task.Dto
{
    public class ProductCreateDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
    }
}
