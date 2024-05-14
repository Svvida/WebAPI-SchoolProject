using Microsoft.EntityFrameworkCore;
using University.Domain.Entities;
using University.Domain.Interfaces;
using University.Infrastructure.Data;

namespace University.Infrastructure.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly UniversityContext _context;
        public StudentRepository(UniversityContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Students>> GetAllStudentsAsync()
        {
            return await _context.Students
                .Include(s=>s.address)
                .ToListAsync();
        }
        public async Task<Students> GetStudentByIdAsync(Guid id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                throw new KeyNotFoundException("That student doesn't exist");
            }

            return student;
        }
        public async Task UpdateStudentAsync(Students student)
        {
            var existingStudent = await _context.Students.FindAsync(student.id);
            if (existingStudent != null)
            {
                _context.Entry(existingStudent).CurrentValues.SetValues(student);
            }
            else
            {
                _context.Students.Update(student);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddStudentAsync(Students student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteStudentAsync(Guid id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                throw new KeyNotFoundException("That student doesn't exist");
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
        }
    }
}
