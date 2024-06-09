using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Addresses
{
    [Authorize(Policy = "EverybodyPolicy")]
    public class IndexModel : PageModel
    {
        private readonly IAddressService _addressService;

        public IndexModel(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public IList<AddressDto> Addresses { get; set; }

        public async Task OnGetAsync()
        {
            Addresses = (await _addressService.GetAllAddressesAsync()).ToList();
        }
    }
}
