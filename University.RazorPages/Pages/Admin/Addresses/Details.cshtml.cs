using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Addresses
{
    [Authorize(Policy = "EverybodyPolicy")]
    public class DetailsModel : PageModel
    {
        private readonly IAddressService _addressService;

        public DetailsModel(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public AddressDto Address { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Address = await _addressService.GetAddressByIdAsync(id);

            if (Address == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
