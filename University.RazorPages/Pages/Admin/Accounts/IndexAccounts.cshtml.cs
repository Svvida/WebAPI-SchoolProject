using Microsoft.AspNetCore.Mvc.RazorPages;
using University.RazorPages.Models;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Admin.Accounts
{
    public class IndexAccountsModel : PageModel
    {
        private readonly GraphQLService _graphQLService;

        public IList<AccountModel> Accounts { get; set; }

        public IndexAccountsModel(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        public async Task OnGetAsync()
        {
            Accounts = await _graphQLService.GetAccountsAsync();
        }
    }
}
