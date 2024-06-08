using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RazorPages.Pages.Admin.Accounts
{
    [Authorize(Policy = "AdminPolicy")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;

        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IList<AccountDto> Accounts { get; set; }

        public async Task OnGetAsync()
        {
            Accounts = (await _accountService.GetAllAccountsAsync()).ToList();
        }
    }
}
