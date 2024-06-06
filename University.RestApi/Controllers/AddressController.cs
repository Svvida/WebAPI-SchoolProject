using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AddressesController : Controller
    {
        private readonly IAddressService _addressService;

        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddressDto>>> GetAll()
        {
            var addresses = await _addressService.GetAllAddressesAsync();
            return Ok(addresses);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AddressDto>> Get(Guid id)
        {
            var address = await _addressService.GetAddressByIdAsync(id);
            if (address is null)
                return NotFound("Address not found");

            return Ok(address);
        }

        [HttpPost]
        public async Task<ActionResult<AddressDto>> Create(AddressDto addressDto)
        {
            await _addressService.CreateAddressAsync(addressDto);
            return CreatedAtAction(nameof(Get), new { id = addressDto.Id }, addressDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, AddressDto addressDto)
        {
            if (id != addressDto.Id)
                return BadRequest();

            var existingAddress = await _addressService.GetAddressByIdAsync(id);
            if (existingAddress is null)
                return NotFound();

            await _addressService.UpdateAddressAsync(addressDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingAddress = await _addressService.GetAddressByIdAsync(id);
            if (existingAddress is null)
                return NotFound();

            await _addressService.DeleteAddressAsync(id);
            return NoContent();
        }
    }
}
