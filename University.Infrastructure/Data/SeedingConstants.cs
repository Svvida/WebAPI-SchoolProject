namespace University.Infrastructure.Data
{
    public static class SeedingConstants
    {
        // Roles ids
        public static readonly Guid AdminRoleId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        public static readonly Guid StudentRoleId = Guid.Parse("00000000-0000-0000-0000-000000000002");
        public static readonly Guid TeacherRoleId = Guid.Parse("00000000-0000-0000-0000-000000000003");
        // Account ids
        public static readonly Guid AdminAccountId = Guid.Parse("00000000-0000-0000-0000-000000000004");
        // Address id for testing
        public static readonly Guid TestAddressId = Guid.Parse("bb7a5062-dd7a-4295-bb29-05a042638467");
    }
}
