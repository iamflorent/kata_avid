using Domain.Enums;

namespace WebApi.ApiModels
{
    public class CreateAdDto
    {
        public string? Title { get; init; }
        public string? Location { get; init; }
        public PropertyType? PropertyType { get; init; }
        public decimal Price { get; set; }
    }
}
