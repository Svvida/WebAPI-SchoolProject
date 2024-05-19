using System;
using System.ComponentModel.DataAnnotations;

namespace University.Domain.Entities
{
    public class Students_Addresses
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Country { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string PostalCode { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string BuildingNumber { get; set; }

        [Required]
        public string ApartmentNumber { get; set; }
    }
}
