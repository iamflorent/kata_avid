using Application.Features.Ad.Commands;
using Application.Features.Ad.Queries;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.ViewModels;

namespace WebUI.Pages
{
    public class IndexModel : PageModel
    {
        
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public CatalogIndexViewModel CatalogModel { get; set; } = new CatalogIndexViewModel();

        public async Task OnGet(CatalogIndexViewModel catalogModel)
        {
            IEnumerable<Domain.Entities.Ad> ads = await _mediator.Send(new GetAdsQuery(catalogModel.TypesFilterApplied));

            CatalogModel = new CatalogIndexViewModel()
            {
                Types = GetTypes(),
                CatalogItems = ads.Select(x =>
                {
                    return new CatalogItemViewModel()
                    {
                        Id = x.Id,
                        Location = x.Location,
                        Title = x.Title,
                        PropertyType = x.PropertyType,
                        Price = x.Price,
                        Status = x.Status
                    };
                }).ToList(),
            };
        }

        public async Task<IActionResult> OnGetPublish(int id)
        {
            await _mediator.Send(new PublishAdCommand(id));

            return LocalRedirect(Url.Content("~/"));
        }

        static PropertyType[] PropertyTypes = (PropertyType[])Enum.GetValues(typeof(PropertyType));
        public List<SelectListItem> GetTypes()
        {

            var items = PropertyTypes
                .Select(type => new SelectListItem() { Value = type.ToString(), Text = type.ToString() })
                .OrderBy(t => t.Text)
                .ToList();

            var allItem = new SelectListItem() { Value = null, Text = "All", Selected = true };
            items.Insert(0, allItem);

            return items;
        }
    }
}