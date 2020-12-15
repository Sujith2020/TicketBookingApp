using System.Collections.Generic;
using System.Threading.Tasks;
using BookingServiceNS.Models;
using BookingServiceNS.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookingServiceNS.Controllers
{
    [Route("api/multiplex")]
    [Authorize]
    [ApiController]
    public class MultiplexController : Controller
    {
        private IMultiplexService _multiplexService;

        public MultiplexController(IMultiplexService multiplexService)
        {
            this._multiplexService = multiplexService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Multiplex>>> GetMultiplexes()
        {
            var Multiplexes = await _multiplexService.GetMultiplexes();
            return Ok(Multiplexes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Multiplex>> GetMultiplex(int id)
        {
            var multiplex = await _multiplexService.GetMultiplex(id);
            if (multiplex == null)
            {
                return NotFound();
            }
            return Ok(multiplex);
        }

        [HttpPost]
        public async Task<ActionResult<Multiplex>> PostMultiplex([FromBody]Multiplex multiplex)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movid = await _multiplexService.PostMultiplex(multiplex);
            return StatusCode(StatusCodes.Status201Created);
            //return Ok(multiplex);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Multiplex>> PutMultiplex(int id, Multiplex multiplex)
        {
            if (id != multiplex.MulId)
            {
                return BadRequest();
            }

            try
            {
                var movid = await _multiplexService.PutMultiplex(id, multiplex);
            }

            catch (DbUpdateConcurrencyException)
            {
                if (!MultiplexExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Multiplex>> DeleteMultiplex(int id)
        {
            var multiplex = await _multiplexService.GetMultiplex(id);
            if (multiplex == null)
            {
                return NotFound();
            }
            var movid = await _multiplexService.DeleteMultiplex(multiplex);
            return Ok();
        }

        private bool MultiplexExists(int id)
        {
            return _multiplexService.MultiplexExists(id);
        }
    }
}
