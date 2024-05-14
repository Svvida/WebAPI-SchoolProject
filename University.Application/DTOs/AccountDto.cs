namespace University.Application.DTOs
{
    public class AccountDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DeactivationDate { get; set; }
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();
    }
}
