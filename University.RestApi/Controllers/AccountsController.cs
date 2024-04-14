using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Services;

namespace University.RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> Get(Guid id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                return Ok(account);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(AccountDto accountDto)
        {
            await _accountService.AddAccountAsync(accountDto);
            return CreatedAtAction(nameof(Get), new { id = accountDto.Id }, accountDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AccountDto accountDto)
        {
            if (id != accountDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _accountService.UpdateAccountAsync(accountDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound();
            }
        }
    }
}
