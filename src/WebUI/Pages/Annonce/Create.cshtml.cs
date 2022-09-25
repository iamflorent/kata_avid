using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MediatR;
using Application.Features.Ad.Commands;

namespace WebUI.Pages.Annonce
{


    public class CreateModel : PageModel
    {

        public class InputModel
        {
            [Required]
            [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "Title")]
            public string? Title { get; init; }
            [Required]
            [StringLength(200, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Location")]
            public string? Location { get; init; }
            [Required]
            [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {1}")]
            [Display(Name = "Price")]
            public decimal Price { get; init; }
            [Required]
            [Display(Name = "PropertyType")]
            public PropertyType? PropertyType { get; init; }

        }

        [BindProperty]
        public InputModel? Input { get; set; }
        public IMediator Mediator { get; }

        public CreateModel(IMediator mediator)
        {
            Mediator = mediator;
        }

        public void OnGet()
        {
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {
                var command = new CreateAdCommand()
                {
                    Location = Input.Location,
                    PropertyType = Input.PropertyType,
                    Title = Input.Title,
                    Price = Input.Price,
                };
                try
                {
                    var result = await Mediator.Send(command);
                    return LocalRedirect(Url.Content("~/"));
                }
                catch (Application.Common.Exceptions.ValidationException exception)
                {
                    
                    foreach (var error in exception.Errors)
                    {
                        var propertyName = error.Key;
                        foreach (var errorDescription in error.Value)
                        {
                            ModelState.AddModelError(propertyName, errorDescription);
                        }
                        
                    }
                }

            }
                       
            return Page();
        }

        PropertyType[] PropertyTypes = (PropertyType[])Enum.GetValues(typeof(PropertyType));
        public List<SelectListItem> GetTypes()
        {

            var items = PropertyTypes
                .Select(type => new SelectListItem() { Value = type.ToString(), Text = type.ToString() })
                .OrderBy(t => t.Text)
                .ToList();

            var allItem = new SelectListItem() { Value = null, Text = "Choose", Selected = true };
            items.Insert(0, allItem);

            return items;
        }
    }
}
