using ConcesionarioApi.Interfaces;
using ConcesionarioApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConcesionarioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcasController : ControllerBase
    {
        private readonly IMarcaRepository _marcaRepository;

        public MarcasController(IMarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Marca>> Get()
        {
            return Ok(_marcaRepository.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<Marca> Get(int id)
        {
            var marca = _marcaRepository.GetById(id);
            if (marca == null) return NotFound();
            return Ok(marca);
        }

        [HttpPost]
        public ActionResult<Marca> Post([FromBody] Marca marca)
        {
            var createdMarca = _marcaRepository.Create(marca);
            return CreatedAtAction(nameof(Get), new { id = createdMarca.Id }, createdMarca);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Marca marca)
        {
            var result = _marcaRepository.Update(id, marca);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _marcaRepository.Delete(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
