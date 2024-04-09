using System.ComponentModel.DataAnnotations;

namespace University.Application.DTOs
{
    public class CreateRoleDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
