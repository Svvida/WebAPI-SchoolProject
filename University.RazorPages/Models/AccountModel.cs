namespace University.RazorPages.Models
{
    public class AccountModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public bool IsActive { get; set; }
        public List<RoleModel> Roles { get; set; }
    }

    public class AccountInputModel
    {
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateAccountInputModel : AccountInputModel
    {
        public Guid Id { get; set; }
    }
}
