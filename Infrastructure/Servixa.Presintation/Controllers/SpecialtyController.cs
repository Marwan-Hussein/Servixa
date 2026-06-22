using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Servixa.Abstractions.Interfaces;
using Servixa.Shared.DTOs.Specialty;

namespace Servixa.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController : ControllerBase
    {
        private readonly ISpecialtyService _specialtyService;

        public SpecialtyController(ISpecialtyService specialtyService)
        {
            _specialtyService = specialtyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _specialtyService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _specialtyService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecialtyDto dto)
        {
            var result = await _specialtyService.CreateAsync(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSpecialtyDto dto)
        {
            var result = await _specialtyService.UpdateAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _specialtyService.DeleteAsync(id);
            return Ok(result);
        }
    }
}
