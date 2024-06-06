using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Interfaces;

namespace University.RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
        {
            var students = await _studentService.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> Get(Guid id)
        {
            var student = await _studentService.GetStudentByIdAsync(id);
            if (student is null)
                return NotFound("Student not found");

            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(StudentDto studentDto)
        {
            await _studentService.AddStudentAsync(studentDto);
            return CreatedAtAction(nameof(Get), new { id = studentDto.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, StudentDto studentDto)
        {
            if (id != studentDto.Id)
                return BadRequest();

            var existingStudent = await _studentService.GetStudentByIdAsync(id);
            if (existingStudent is null)
                return NotFound();

            await _studentService.UpdateStudentAsync(studentDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existingStudent = await _studentService.GetStudentByIdAsync(id);
            if (existingStudent is null)
                return NotFound();

            await _studentService.DeleteStudentAsync(id);
            return NoContent();
        }
    }
}
