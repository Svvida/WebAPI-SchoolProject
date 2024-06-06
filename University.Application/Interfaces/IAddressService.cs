using University.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace University.Application.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetAllAddressesAsync();
        Task<AddressDto> GetAddressByIdAsync(Guid id);
        Task<AddressDto> CreateAddressAsync(AddressDto addressDto);
        Task<AddressDto> UpdateAddressAsync(AddressDto addressDto);
        Task DeleteAddressAsync(Guid id);
    }
}
