using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;

        public StudentController(ILogger<StudentController> logger)
        {
            _logger = logger;
        }


        [HttpGet("All")]
        [ProducesResponseType(200)]
        public ActionResult<IEnumerable<StudentDTO>> GetStudents()
        {
            _logger.LogInformation("GetStudents method started");
            var students = CollegeRepository.Students.Select(s => new StudentDTO()
            {
                Id = s.Id,
                StudentName = s.StudentName,
                Address = s.Address,
                Email = s.Email
            });
            return Ok(students);


        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<StudentDTO> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad request");
                return BadRequest();
            }

            var student = CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            if (student == null)
            {
                _logger.LogError("Student not found with given Id");
                return NotFound($"Student with id {id} was not found.");
            }

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
            };
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<StudentDTO> GetStudentByName(string name)
        {
            if(string.IsNullOrEmpty(name)) return BadRequest();
            
            var student = CollegeRepository.Students.Where(n => n.StudentName == name).FirstOrDefault();

            if(student == null) return NotFound($"The student with name {name} was not found.");

            var studentDTO = new StudentDTO
            {
                Id = student.Id,
                StudentName = student.StudentName,
                Address = student.Address,
                Email = student.Email,
            };
            return Ok(studentDTO); 
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public ActionResult<bool> DeleteStudentById(int id)
        {
            if (id <= 0) return BadRequest();

            var student =  CollegeRepository.Students.Where(n => n.Id == id).FirstOrDefault();

            if (student == null) return NotFound($"Student with id {id} was not found.");

            CollegeRepository.Students.Remove(student);

            return Ok(true);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public ActionResult<StudentDTO> CreateStudent(StudentDTO model)
        {
            if (model == null) return BadRequest();
            int newid = CollegeRepository.Students.LastOrDefault().Id + 1;
            Student student = new Student
            {
                Id = newid,
                StudentName = model.StudentName,
                Address = model.Address,
                Email = model.Email,
            };
            CollegeRepository.Students.Add(student);

            model.Id = student.Id;
            return CreatedAtRoute("GetStudentById", new { id = model.Id }, model);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public ActionResult UpdateStudent([FromBody] StudentDTO model)
        {
            if (model == null || model.Id <= 0) return BadRequest();

            var existingStudent = CollegeRepository.Students.Where(n => n.Id == model.Id).FirstOrDefault();

            if(existingStudent == null) return BadRequest();

            existingStudent.StudentName = model.StudentName;
            existingStudent.Address = model.Address;
            existingStudent.Email = model.Email;

            return NoContent();
        }

    }
}
