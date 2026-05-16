using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private static readonly List<Student> Students =
        [
            new Student { Id = 1, Name = "Alice", Age = 21, Major = "Computer Science" },
            new Student { Id = 2, Name = "Bob", Age = 22, Major = "Information Technology" }
        ];

        [HttpGet]
        public ActionResult<List<Student>> Get() => Students;

        [HttpPost]
        public IActionResult Create([FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            student.Id = Students.Count == 0 ? 1 : Students.Max(s => s.Id) + 1;
            Students.Add(student);
            return CreatedAtAction(nameof(Get), new { id = student.Id }, student);
        }

        [HttpPut("{id:int}")]
        public IActionResult Update(int id, [FromBody] Student student)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var existing = Students.FirstOrDefault(s => s.Id == id);
            if (existing is null)
            {
                return NotFound();
            }

            existing.Name = student.Name;
            existing.Age = student.Age;
            existing.Major = student.Major;
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            var student = Students.FirstOrDefault(s => s.Id == id);
            if (student is null)
            {
                return NotFound();
            }

            Students.Remove(student);
            return NoContent();
        }
    }
}
