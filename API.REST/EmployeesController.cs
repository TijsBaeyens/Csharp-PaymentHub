using AutoMapper;
using EmpAPI1.Domain.Interfaces.Repositories;
using EmpAPI1.Domain.Models;
using EmpAPI1.REST.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

#if ProducesConsumes
using System.Net.Mime;
#endif

namespace EmpAPI1.REST
{

    [Route("api/[controller]")]
    [ApiController]
#if ProducesConsumes
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
#endif
    public class EmployeesController : ControllerBase
    {
        private readonly IEmpRepository _empRepository;
        private readonly IMapper _mapper;

        public EmployeesController(IEmpRepository empRepository, IMapper mapper)
        {
            _mapper = mapper;
            _empRepository = empRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetEmployees()
        {
            return Ok(await _empRepository.Get());
        }

        [HttpGet("{EmpId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<UserDTO>> GetEmployees(int EmpId)
        {
            return Ok(await _empRepository.Get(EmpId));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<UserDTO>> PostEmployees([FromBody] UserDTO employee)
        {
            var newEmployee = await _empRepository.Create(_mapper.Map<Employee>(employee));
            return CreatedAtAction(nameof(GetEmployees), new { newEmployee.EmpId }, newEmployee);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PutEmployees(int EmpId, [FromBody] UserDTO employee)
        {
            // Check if the given id is present database or not; if not then we will return bad request
            if (EmpId != employee.EmpId)
            {
                return BadRequest();
            }
            await _empRepository.Update(_mapper.Map<Employee>(employee));
            return NoContent();
        }

        [HttpDelete("{EmpId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Delete(int EmpId)
        {
            var employeeToDelete = await _empRepository.Get(EmpId);
            // We will check if the given id is present in database or not
            if (employeeToDelete == null)
                return NotFound();
            await _empRepository.Delete(employeeToDelete.EmpId);
            return NoContent();
        }
    }
}