namespace University.RazorPages.Models
{
    public class AddressModel
    {
        public Guid Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string Street { get; set; }
        public string BuildingNumber { get; set; }
        public string ApartmentNumber { get; set; }
    }
}
