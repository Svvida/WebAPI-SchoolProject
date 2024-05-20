using Microsoft.AspNetCore.Mvc.RazorPages;
using University.RazorPages.Models;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Admin.Addresses
{
    public class IndexAddressesModel : PageModel
    {
        private readonly GraphQLService _graphQLService;
        public IList<AddressModel> Addresses { get; private set; }

        public IndexAddressesModel(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        public async Task OnGetAsync()
        {
            Addresses = await _graphQLService.GetAddressesAsync();
        }
    }
}
