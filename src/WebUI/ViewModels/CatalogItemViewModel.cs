using Domain.Enums;

namespace WebUI.ViewModels;

public class CatalogItemViewModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Location { get; set; }
    public decimal Price { get; set; }
    public PropertyType PropertyType { get; set; }
    public Status Status { get; set; }
}
