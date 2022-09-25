using Application.Common.Exceptions;
using Application.Features.Ad.Commands;
using Application.Features.Ad.Queries;
using Application.Features.Location.Queries.GetMeteoInfo;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebUI.ViewModels;

namespace WebUI.Pages.Annonce
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IMediator mediator)
        {
            _mediator = mediator;
        }

        public CatalogItemViewModel CatalogItemViewModel { get; set; }
        public MeteoViewModel MeteoInfo { get; set; }

        public async Task<IActionResult> OnGet(int id)
        {
            try
            {
                var ad = await _mediator.Send(new GetAdQuery(id));

                CatalogItemViewModel = new CatalogItemViewModel()
                {
                    Id = id,
                    Location = ad.Location,
                    Price = ad.Price,
                    PropertyType = ad.PropertyType,
                    Status = ad.Status,
                    Title = ad.Title
                };

                try
                {
                    var meteoInfo = await _mediator.Send(new GetMeteoInfoQuery(ad.Location));
                    MeteoInfo = new MeteoViewModel(ad.Location, meteoInfo.Temperature, meteoInfo.Weathercode);
                }
                catch (NotFoundException)
                {

                    MeteoInfo = new MeteoViewModel($"sry, we can't find meteo for '{ad.Location}' :')");
                }
            }
            catch (NotFoundException)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
