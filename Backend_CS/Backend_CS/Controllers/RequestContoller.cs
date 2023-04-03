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
    public class RequestContoller : ControllerBase
    {
        private readonly TableContext _context;

        public RequestContoller(TableContext context)
        {
            _context = context;
        }

        // GET: api/RequestContoller
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequests()
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }

            return await _context.Requests.Include(rd => rd.requestData).ToListAsync();
        }

        // GET: api/RequestContoller/5
        [HttpGet("StatusNumber/WorkGroupId")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest(int statusNumber, int workGroupId)
        {
          if (_context.Requests == null)
          {
              return NotFound();
          }
            var request = await _context.Requests.Where(r => r.requestData.statusNumber == statusNumber && r.workGroupId == workGroupId).Include(r => r.requestData).ToListAsync();

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/RequestContoller/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRequest(int id, Request request)
        {
            if (id != request.id)
            {
                return BadRequest();
            }

            _context.Entry(request).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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

        // POST: api/RequestContoller
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Request>> PostRequest(Request request)
        {
            if (_context.Requests == null)
            {
                return Problem("Entity set 'TableContext.Requests'  is null.");
            }

            if (_context.WorkGroups.FirstOrDefault(wg => wg.id == request.workGroupId) == null)
            {
                return Problem("You don't have workGroup with this id");
            }

            if (_context.Workers.FirstOrDefault(w => w.id == request.requestData.userId) == null)
            {
                request.requestData.userId = null;
            }

            var Request1 = new Request(request.workGroupId, request.id, request.requestData.name, request.requestData.price, request.requestData.userId, request.requestData.priorityId, request.requestData.statusNumber);
            var requests = _context.Requests.Where(r => r.workGroupId == Request1.workGroupId && r.requestData.statusNumber == Request1.requestData.statusNumber).Include(rd => rd.requestData).ToList();

            if (requests != null)
            {
                var i = 0;
                requests.ForEach(r => { i += 1; r.requestData.statusPosition = i; });
                await _context.SaveChangesAsync();
            }
            _context.Requests.Add(Request1);
            
            var workGroup = _context.WorkGroups.FirstOrDefault(wg => wg.id == request.workGroupId);
            if (workGroup != null)
            {
                workGroup.requestsData.Add(Request1.requestData);
                await _context.SaveChangesAsync();
            }
            await _context.SaveChangesAsync();

            return Ok();
            
        }

        // DELETE: api/RequestContoller/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}
