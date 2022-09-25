using Domain.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.ViewModels;

public class CatalogIndexViewModel
{
    public List<CatalogItemViewModel>? CatalogItems { get; set; }
    public PropertyType? TypesFilterApplied { get; set; }
    public List<SelectListItem>? Types { get; set; }
}
