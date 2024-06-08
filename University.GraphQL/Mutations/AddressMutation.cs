using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Mutations
{
    public class AddressInput
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
    }
    public class UpdateAddressInput
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
    }

    public class AddressMutation
    {
        public async Task<Students_Addresses> AddAddressAsync(AddressInput input, [Service] UniversityContext context)
        {
            var address = new Students_Addresses
            {
                Id = Guid.NewGuid(),
                Country = input.Country,
                City = input.City,
                PostalCode = input.PostalCode,
                Street = input.Street,
                BuildingNumber = input.BuildingNumber,
                ApartmentNumber = input.ApartmentNumber
            };

            context.Addresses.Add(address);
            await context.SaveChangesAsync();
            return address;
        }
        public async Task<Students_Addresses> UpdateAddressAsync(UpdateAddressInput input, [Service] UniversityContext context)
        {
            var address = await context.Addresses.FindAsync(input.Id);
            if (address == null)
            {
                throw new Exception($"Address with ID {input.Id} not found");
            }

            address.Country = input.Country;
            address.City = input.City;
            address.PostalCode = input.PostalCode;
            address.Street = input.Street;
            address.BuildingNumber = input.BuildingNumber;
            address.ApartmentNumber = input.ApartmentNumber;

            context.Addresses.Update(address);
            await context.SaveChangesAsync();
            return address;
        }
        public async Task<bool> DeleteAddressAsync(Guid id, [Service] UniversityContext context)
        {
            var address = await context.Addresses.FindAsync(id);
            if (address == null)
            {
                return false;
            }

            context.Addresses.Remove(address);
            await context.SaveChangesAsync();
            return true;
        }
    }
}
