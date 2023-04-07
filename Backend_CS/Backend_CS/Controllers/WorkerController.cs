using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_CS.Models;
using Backend_CS.assets;
using Backend_CS.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace Backend_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly TableContext _context;

        public WorkerController(TableContext context)
        {
            _context = context;
        }

        // GET: api/Worker
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Worker>>> GetWorkers()
        {
          if (_context.Workers == null)
          {
              return NotFound();
          }
            return await _context.Workers.ToListAsync();
        }

        // GET: api/Worker/5
        [HttpGet("GetWorkerRequests")]
        public async Task<ActionResult<IEnumerable<Request>>> GetWorker(int id)
        {
          if (_context.Workers == null)
          {
              return NotFound();
          }
            var workerRequests = _context.Requests.Include(rds => rds.requestData).Where(r => r.requestData.userId == id).ToList();

            if (workerRequests == null)
            {
                return NotFound();
            }

            return workerRequests;
        }

        // PUT: api/Worker/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorker(int id, Worker worker)
        {
            if (id != worker.id)
            {
                return BadRequest();
            }

            _context.Entry(worker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkerExists(id))
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

        // POST: api/Worker
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Worker>> PostWorker(WorkerDTO workerDTO)
        {
          if (_context.Workers == null)
          {
              return Problem("Entity set 'TableContext.Workers'  is null.");
          }
            var worker = new Worker(workerDTO.name, workerDTO.imgUrl, workerDTO.password);
            _context.Workers.Add(worker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorker", new { id = worker.id }, worker);
        }

        // DELETE: api/Worker/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            if (_context.Workers == null)
            {
                return NotFound();
            }
            var worker = await _context.Workers.FindAsync(id);
            if (worker == null)
            {
                return NotFound();
            }

            _context.Workers.Remove(worker);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WorkerExists(int id)
        {
            return (_context.Workers?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
