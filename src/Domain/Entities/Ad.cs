using Domain.Enums;

namespace Domain.Entities
{
    public class Ad
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }    
        public decimal Price { get; set; }
        public string Location { get; set; }
        public PropertyType PropertyType { get; set;}
        public Status Status { get; set; }

    }
}