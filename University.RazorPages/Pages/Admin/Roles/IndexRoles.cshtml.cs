using Microsoft.AspNetCore.Mvc.RazorPages;
using University.RazorPages.Models;
using University.RazorPages.Services;

namespace University.RazorPages.Pages.Admin.Roles
{
    public class IndexRolesModel : PageModel
    {
        private readonly GraphQLService _graphQLService;

        public IList<RoleModel> Roles { get; set; }

        public IndexRolesModel(GraphQLService graphQLService)
        {
            _graphQLService = graphQLService;
        }

        public async Task OnGetAsync()
        {
            Roles = await _graphQLService.GetRolesAsync();
        }
    }
}
