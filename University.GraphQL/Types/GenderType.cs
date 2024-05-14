using University.Domain.Enums;

namespace University.GraphQL.Types
{
    public class GenderType : EnumType<Gender>
    {
        protected override void Configure(IEnumTypeDescriptor<Gender> descriptor)
        {
            descriptor.Name("Gender");
            descriptor.Value(Gender.Female).Name("Female");
            descriptor.Value(Gender.Male).Name("Male");
            descriptor.Value(Gender.Other).Name("Other");
        }
    }
}
