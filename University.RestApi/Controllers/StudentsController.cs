using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Services;

namespace University.RestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly StudentService _studentSerivce;

        public StudentsController(StudentService studentService)
        {
            _studentSerivce = studentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAll()
        {
            var students = await _studentSerivce.GetAllStudentsAsync();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<StudentDto>> Get(Guid id)
        {
            try
            {
                var student = await _studentSerivce.GetStudentByIdAsync(id);
                return Ok(student);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<StudentDto>> Create(StudentDto studentDto)
        {
            await _studentSerivce.AddStudentAsync(studentDto);
            return CreatedAtAction(nameof(Get), new { id = studentDto.Id }, studentDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, StudentDto studentDto)
        {
            if (id != studentDto.Id)
            {
                return BadRequest();
            }

            try
            {
                await _studentSerivce.UpdateStudentAsync(studentDto);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            { return NotFound(); }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _studentSerivce.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            { return NotFound(); }
        }
    }
}
