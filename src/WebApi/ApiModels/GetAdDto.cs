using Domain.Enums;

namespace WebApi.ApiModels
{
    public class GetAdDto
    {
        public GetAdDto()
        {
            Title = string.Empty;
            Location = string.Empty;
        }
        public GetAdDto(int id, string title, string location, PropertyType propertyType, Status status)
        {
            Id = id;
            Title = title;
            Location = location;
            PropertyType = propertyType;
            Status = status;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public PropertyType PropertyType { get; set; }
        public Status Status { get; set; }
    }
}
