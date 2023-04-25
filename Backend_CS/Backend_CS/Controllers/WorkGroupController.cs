using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Backend_CS.Models;
using Backend_CS.assets;
using Backend_CS.Models.DTO;

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

        [HttpGet("GetGroupWithMoreRequests")]
        public async Task<ActionResult<WorkGroup>> GetWorkGroup()
        {
            if (_context.WorkGroups == null || _context.WorkGroups.ToList().Count == 0)
            {
                return NotFound();
            }
            var workGroup = _context.WorkGroups
                .Include(rds => rds.requestsData)
                .OrderByDescending(wg => wg.requestsData.Count())
                .ToList()[0];
            


            return workGroup;
        }

        [HttpGet("GetNumberOfRequestsInWorkGroup")]
        public async Task<ActionResult<int>> GetWorkGroups(int workGroupId)
        {
            if (_context.WorkGroups == null || _context.WorkGroups.FirstOrDefault(w => w.id == workGroupId) == null )
            {
                return NotFound();
            }
            var requests = _context.Requests.Where(r => r.workGroupId == workGroupId).ToList();
            

            return requests.Count;
        }

        // GET: api/WorkGroup/5
        [HttpGet("GetWorkGroupRequests")]
        public async Task<ActionResult<IEnumerable<RequestData>>> GetWorkGroup(int id)
        {
          if (_context.WorkGroups == null)
          {
              return NotFound();
          }
            var workGroup = await _context.WorkGroups.Include(rds => rds.requestsData).FirstOrDefaultAsync(w => w.id == id);

            if (workGroup == null)
            {
                return NotFound();
            }

            return workGroup.requestsData.ToList();
        }

        // PUT: api/WorkGroup/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
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
        //[Authorize(Roles = "admin")]
        public async Task<ActionResult<WorkGroup>> PostWorkGroup(PostWorkGroupDTO postWorkGroupDTO)
        {
          if (_context.WorkGroups == null)
          {
              return Problem("Entity set 'TableContext.WorkGroups'  is null.");
          }
            var workGroup = new WorkGroup(0, postWorkGroupDTO.name, new List<RequestData>());
            _context.WorkGroups.Add(workGroup);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWorkGroup", new { id = workGroup.id }, workGroup);
        }

        // DELETE: api/WorkGroup/5
        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteWorkGroup(int id)
        {
            if (_context.WorkGroups == null)
            {
                return NotFound();
            }
            var workGroup = _context.WorkGroups.Include(rd => rd.requestsData).FirstOrDefault(w => w.id == id);
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
