using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using University.Application.DTOs;

namespace University.Application.Interfaces
{
    public interface IStudentService
    {
        Task<IEnumerable<StudentDto>> GetAllStudentsAsync();
        Task<StudentDto> GetStudentByIdAsync(Guid id);
        Task AddStudentAsync(StudentDto studentDto);
        Task DeleteStudentAsync(Guid id);
        Task UpdateStudentAsync(StudentDto studentDto);
    }
}
