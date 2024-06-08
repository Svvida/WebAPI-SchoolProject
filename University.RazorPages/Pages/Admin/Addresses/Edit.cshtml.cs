using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Addresses
{
    [Authorize(Policy = "AdminPolicy")]
    public class EditModel : PageModel
    {
        private readonly IAddressService _addressService;

        public EditModel(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _addressService.UpdateAddressAsync(Address);
            return RedirectToPage("./Index");
        }
    }
}
