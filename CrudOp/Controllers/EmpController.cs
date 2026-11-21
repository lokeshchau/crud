using CrudOp.Models;
using CrudOp.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CrudOp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpController : ControllerBase
    {
        private readonly IEmp _emp;

        public EmpController(IEmp emp)
        {
            _emp = emp;
        }

        [HttpPost]
        public async Task<IActionResult> AddEmp([FromBody] Employee employee)
        {
            await _emp.add(employee);
            return Ok("Successfully Added employee");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var res = await _emp.findAll();
            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var res = await _emp.getById(id);
            return Ok(res);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmp(int id)
        {
            await _emp.delete(id);
            return Ok("Deleted Successfully");
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Employee employee)
        {
            var val = await _emp.update(employee);
            return Ok(val);
        }
    }
}
