using University.Domain.Entities;
using University.Infrastructure.Data;

namespace University.GraphQL.Mutations
{
    public class AddressInput
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
        public async Task<Students_Addresses> UpdateAddressAsync(AddressInput input, [Service] UniversityContext context)
        {
            var address = await context.Addresses.FindAsync(input.Id);
            if (address == null)
            {
                throw new Exception($"Address with ID {input.Id} not found");
            }

            address.country = input.Country;
            address.city = input.City;
            address.postal_code = input.PostalCode;
            address.street = input.Street;
            address.building_number = input.BuildingNumber;
            address.apartment_number = input.ApartmentNumber;

            context.Addresses.Update(address);
            await context.SaveChangesAsync();
            return address;
        }
    }
}
