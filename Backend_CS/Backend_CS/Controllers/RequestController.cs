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
    public class RequestController : ControllerBase
    {
        private readonly TableContext _context;

        public RequestController(TableContext context)
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

        [HttpGet("GetRequestsById")]
        public async Task<ActionResult<Request>> GetRequests(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }

            var request = await _context.Requests.Include(rd => rd.requestData).FirstOrDefaultAsync(r => r.id == id);

            return request;
        }

        // GET: api/RequestContoller/5
        [HttpGet("GetRequestsByWorkGroupAndStatus")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest(int workGroupId, int statusNumber)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var requests = await _context.Requests
                .Where(r => r.requestData.statusNumber == statusNumber && r.workGroupId == workGroupId)
                .Include(r => r.requestData)
                .ToListAsync();

            if (requests == null)
            {
                return NotFound();
            }

            return requests;
        }

        [HttpGet("GetRequestsByWorkGroupByName")]
        public async Task<ActionResult<IEnumerable<Request>>> GetRequest(int workGroupId, string name)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests
                .Where(r => r.requestData.name == name && r.workGroupId == workGroupId)
                .Include(r => r.requestData)
                .ToListAsync();

            if (request == null)
            {
                return NotFound();
            }

            return request;
        }

        // PUT: api/RequestContoller/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("EditRequest")]
        public async Task<IActionResult> PutRequest(int id, EditRequestDTO editRequestData)
        {
            var requestData = _context.requestDatas.FirstOrDefault(r => r.id == id);
            if (requestData == null)
            {
                return Problem("You don't have request with this id");
            }
            var workGroupId = _context.Requests.FirstOrDefault(r => r.id == id).workGroupId;
            var requestsStatus = _context.Requests
                .Where(r => r.workGroupId == workGroupId && r.requestData.statusNumber == editRequestData.statusNumber)
                .Include(rd => rd.requestData)
                .OrderBy(r => r.requestData.statusPosition)
                .ToList();

            if (_context.Workers.FirstOrDefault(w => w.id == editRequestData.userId) != null)
            {
                requestData.userId = editRequestData.userId;
            }
            else if (editRequestData.userId == null)
            {
                requestData.userId = null;
            }

            requestData.lastUpdateDate = DateTime.Now;
            requestData.name = editRequestData.name;
            requestData.price = editRequestData.price;
            requestData.priorityId = editRequestData.priorityId;
            if (editRequestData.comment == "string")
            {
                requestData.comment = "";
            } else
            {
                requestData.comment = editRequestData.comment;
            }
            if (requestData.statusNumber != editRequestData.statusNumber)
            {
                if (requestsStatus != null)
                {
                    var i = 0;
                    requestsStatus.ForEach(r => { i += 1; r.requestData.statusPosition = i; });
                    await _context.SaveChangesAsync();
                }
                requestData.statusNumber = editRequestData.statusNumber;
                requestData.statusPosition = 0;
            }

            _context.Entry(requestData).State = EntityState.Modified;

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

        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> PutRequest(int id, int statusNumber, int statusPosition)
        {
            var requestData = _context.requestDatas.FirstOrDefault(r => r.id == id);
            var workGroupId = _context.Requests.FirstOrDefault(r => r.id == id).workGroupId;
            var requestsStatus = _context.Requests
                .Where(r => r.workGroupId == workGroupId && r.requestData.statusNumber == statusNumber)
                .Include(rd => rd.requestData)
                .OrderBy(r => r.requestData.statusPosition)
                .ToList();

            if (requestData == null)
            {
                return Problem("You don't have request with this id");
            }
            if (requestData.statusNumber != statusNumber)
            {
                if (requestsStatus != null)
                {
                    var i = 0;
                    requestsStatus.ForEach(r =>
                    {
                        if (i == statusPosition)
                        {
                            i += 1;
                        }
                        r.requestData.statusPosition = i;
                        i += 1;

                    });
                    await _context.SaveChangesAsync();
                }
                requestData.statusNumber = statusNumber;
                requestData.statusPosition = statusPosition;
            }

            _context.Entry(requestData).State = EntityState.Modified;

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
        [Authorize(Roles = "admin")]
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
            request.requestData.statusNumber = 0;
            var Request1 = new Request(request.workGroupId, request.id, request.requestData.name, request.requestData.price, request.requestData.userId, request.requestData.priorityId, request.requestData.statusNumber);
            var requests = _context.Requests
                .Where(r => r.workGroupId == Request1.workGroupId && r.requestData.statusNumber == Request1.requestData.statusNumber)
                .Include(rd => rd.requestData)
                .ToList();

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
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteRequest(int id)
        {
            if (_context.Requests == null)
            {
                return NotFound();
            }
            var request = await _context.Requests.FindAsync(id);
            var requestData = await _context.requestDatas.FindAsync(id);
            if (request == null)
            {
                return NotFound();
            }

            _context.Requests.Remove(request);
            await _context.SaveChangesAsync();

            _context.requestDatas.Remove(requestData);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RequestExists(int id)
        {
            return (_context.Requests?.Any(e => e.id == id)).GetValueOrDefault();
        }
    }
}


