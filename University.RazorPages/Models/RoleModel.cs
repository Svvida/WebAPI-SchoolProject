namespace University.RazorPages.Models
{
    public class RoleModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class RoleInputModel
    {
        public string Name { get; set; }
    }
}
