using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Backend_CS.Models;
using Backend_CS.assets;

namespace Backend_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkGroupController : ControllerBase
    {
        private readonly TableContext _context;

        public WorkGroupController(TableContext context)
        {
            _context = context;
        }

        // GET: api/WorkGroup
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WorkGroup>>> GetWorkGroups()
        {
          if (_context.WorkGroups == null)
          {
              return NotFound();
          }
            return await _context.WorkGroups.Include(rds => rds.requestsData).ToListAsync();
        }

        // GET: api/WorkGroup/5
        [HttpGet("{id}")]
        public async Task<ActionResult<WorkGroup>> GetWorkGroup(int id)
        {
          if (_context.WorkGroups == null)
          {
              return NotFound();
          }
            var workGroup = await _context.WorkGroups.FindAsync(id);

            if (workGroup == null)
            {
                return NotFound();
            }

            return workGroup;
        }

        // PUT: api/WorkGroup/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWorkGroup(int id, WorkGroup workGroup)
        {
            if (id != workGroup.id)
            {
                return BadRequest();
            }

            _context.Entry(workGroup).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WorkGroupExists(id))
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

        // POST: api/WorkGroup
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<WorkGroup>> PostWorkGroup(WorkGroup workGroup)
        {
          if (_context.WorkGroups == null)
          {
              return Problem("Entity set 'TableContext.WorkGroups'  is null.");
          }
            workGroup.requestsData = new List<RequestData> ();
            _context.WorkGroups.Add(workGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkGroup", new { id = workGroup.id }, workGroup);
        }

        // DELETE: api/WorkGroup/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkGroup(int id)
        {
            if (_context.WorkGroups == null)
            {
                return NotFound();
            }
            var workGroup = await _context.WorkGroups.FindAsync(id);
            if (workGroup == null)
            {
                return NotFound();
            }

            _context.WorkGroups.Remove(workGroup);
            await _context.SaveChangesAsync();

            

            return NoContent();

        }

        private bool WorkGroupExists(int id)
        {
            return (_context.WorkGroups?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
