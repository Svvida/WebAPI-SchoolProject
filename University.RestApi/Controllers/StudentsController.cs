using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Interfaces;
using University.Application.Services;
using University.Domain.Interfaces;

namespace University.RestApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : Controller
    {
        private readonly IStudentService _studentSerivce;

        public StudentsController(IStudentService studentService)
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
