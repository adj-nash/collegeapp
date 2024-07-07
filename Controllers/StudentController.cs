using Microsoft.AspNetCore.Mvc;
using CollegeApp.Models;
using CollegeApp.Data;
using AutoMapper;
using CollegeApp.Data.Repository;

namespace CollegeApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;

        public StudentController(ILogger<StudentController> logger, IMapper mapper, IStudentRepository studentRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _studentRepository = studentRepository;

        }


        [HttpGet("All")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<IEnumerable<StudentDTO>>> GetStudentsAsync()
        {
            _logger.LogInformation("GetStudents method started");
            var students = await _studentRepository.GetAllAsync();
            var studentDTOData = _mapper.Map<List<StudentDTO>>(students); 
            return Ok(studentDTOData);


        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StudentDTO>> GetStudentById(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Bad request");
                return BadRequest();
            }

            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                _logger.LogError("Student not found with given Id");
                return NotFound($"Student with id {id} was not found.");
            }

            var studentDTO = _mapper.Map<StudentDTO>(student);
            return Ok(studentDTO);
        }

        [HttpGet("{name:alpha}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<StudentDTO>> GetStudentByName(string name)
        {
            if(string.IsNullOrEmpty(name)) return BadRequest();
            
            var student = await _studentRepository.GetByNameAsync(name);

            if(student == null) return NotFound($"The student with name {name} was not found.");

            var studentDTO = _mapper.Map<StudentDTO>(student);

            return Ok(studentDTO); 
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> DeleteStudentById(int id)
        {
            if (id <= 0) return BadRequest();

            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null) return NotFound($"Student with id {id} was not found.");

            await _studentRepository.DeleteAsync(student);
            return Ok(true);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]

        public async Task<ActionResult<StudentDTO>> CreateStudent(StudentDTO dto)
        {
            if (dto == null) return BadRequest();
            Student student = _mapper.Map<Student>(dto);
            var id = await _studentRepository.CreateAsync(student);

            dto.Id = id;
            return CreatedAtRoute("GetStudentById", new { id = dto.Id }, dto);
        }

        [HttpPut]
        [Route("Update")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]

        public async Task<ActionResult> UpdateStudent([FromBody] StudentDTO dto)
        {
            if (dto == null || dto.Id <= 0) return BadRequest();

            var existingStudent = await _studentRepository.GetByIdAsync(dto.Id, true);

            if(existingStudent == null) return NotFound();

            var newRecord = _mapper.Map<Student>(dto);

            await _studentRepository.UpdateAsync(newRecord);

            return NoContent();
        }

    


    }
}
