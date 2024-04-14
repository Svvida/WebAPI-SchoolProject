using AutoMapper;
using University.Application.DTOs;
using University.Application.NewFolder;
using University.Domain.Entities;
using University.Domain.Interfaces;

namespace University.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllStudentsAsync();

            return _mapper.Map<IEnumerable<StudentDto>>(students);
        }

        public async Task<StudentDto> GetStudentByIdAsync(Guid id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);

            if (student == null)
            {
                throw new KeyNotFoundException("That student doesn't exist");
            }

            return _mapper.Map<StudentDto>(student);
        }
        public async Task AddStudentAsync(StudentDto studentDto)
        {
            var student = _mapper.Map<Students>(studentDto);
            await _studentRepository.AddStudentAsync(student);
        }
        public async Task UpdateStudentAsync(StudentDto studentDto)
        {
            var student = _mapper.Map<Students>(studentDto);
            await _studentRepository.UpdateStudentAsync(student);
        }
        public async Task DeleteStudentAsync(Guid id)
        {
            var student = await _studentRepository.GetStudentByIdAsync(id);
            if (student == null)
            {
                throw new KeyNotFoundException("Student not found");
            }

            await _studentRepository.DeleteStudentAsync(student.id);
        }
    }
}
