using AutoMapper;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Domain.Entities;
using University.Domain.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace University.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AddressDto>> GetAllAddressesAsync()
        {
            var addresses = await _addressRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AddressDto>>(addresses);
        }

        public async Task<AddressDto> GetAddressByIdAsync(Guid id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address is null)
            {
                throw new KeyNotFoundException("Address not found");
            }

            return _mapper.Map<AddressDto>(address);
        }

        public async Task<AddressDto> CreateAddressAsync(AddressDto addressDto)
        {
            var addressEntity = _mapper.Map<Students_Addresses>(addressDto);
            var createdAddress = await _addressRepository.CreateAsync(addressEntity);
            return _mapper.Map<AddressDto>(createdAddress);
        }

        public async Task<AddressDto> UpdateAddressAsync(AddressDto addressDto)
        {
            var addressEntity = await _addressRepository.GetByIdAsync(addressDto.Id);
            if (addressEntity is null)
            {
                throw new KeyNotFoundException("Address not found");
            }

            _mapper.Map(addressDto, addressEntity);
            var updatedAddress = await _addressRepository.UpdateAsync(addressEntity);
            return _mapper.Map<AddressDto>(updatedAddress);
        }

        public async Task DeleteAddressAsync(Guid id)
        {
            await _addressRepository.DeleteAsync(id);
        }
    }
}
