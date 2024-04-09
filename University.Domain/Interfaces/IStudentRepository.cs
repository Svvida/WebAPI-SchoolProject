using University.Domain.Entities;

namespace University.Domain.Interfaces
{
    public interface IStudentRepository
    {
        public Task<IEnumerable<Students>> GetAllStudentsAsync();
        public Task<Students> GetStudentByIdAsync(Guid id);
        public Task AddStudentAsync(Students student);
        public Task DeleteStudentAsync(Guid id);
        public Task UpdateStudentAsync(Students student);
    }
}
