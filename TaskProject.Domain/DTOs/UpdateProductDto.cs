using Microsoft.AspNetCore.Http;

namespace TaskProject.Domain.DTOs
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile? VideoPath { get; set; }
        public string SortNumber { get; set; }
    }
}