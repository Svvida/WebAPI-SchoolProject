using System.ComponentModel.DataAnnotations;

namespace University.Domain.Entities
{
    public class Students_Addresses
    {
        [Required]
        public Guid id { get; set; }
        [Required]
        public string country { get; set; }
        [Required]
        public string city { get; set; }
        [Required]
        public string postal_code { get; set; }
        [Required]
        public string street { get; set; }
        [Required]
        public string building_number { get; set; }
        [Required]
        public string apartment_number { get; set; }

        public Students student { get; set; }
    }
}
