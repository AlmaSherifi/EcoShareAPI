using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using APIAlma.Data;
using APIAlma.Models;
using System.Formats.Asn1;

namespace APIAlma
{
    [Route("api/[controller]")]
    [ApiController]

    public class ToolsController : ControllerBase
    {
        private readonly ApiDbContext _context;

        public ToolsController(ApiDbContext context)
        {
            _context = context;
        }

       // GET: api/Tools
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tool>>> GetTools()
        {
            return await _context.Tools.ToListAsync();
        }

        // GET: api/Tools/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tool>> GetTool(int id)
        {
            var tool = await _context.Tools.FindAsync(id);

            if (tool == null)
            {
                return NotFound();
            }

            return tool;
        }

        // POST: api/Tools
        [HttpPost, Authorize]
        public async Task<ActionResult<Tool>> PostTool(Tool tool)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if(userId == null)
            {
                return BadRequest("Unable to determine user identity.");
            }
            tool.UserId = userId;
            
            _context.Tools.Add(tool);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTool), new { id = tool.Id }, tool);
        }

        //PUT: api/Tools
        [HttpPut("{id}"), Authorize]
        public async Task<IActionResult> PutTool(int id, Tool updatedTool)
        {
            if (id != updatedTool.Id)
            {
                return BadRequest();
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userOwnsTool = await _context.Tools.AnyAsync(t => t.Id == id && t.UserId == userId);

            if (!userOwnsTool)
            {
                return Forbid(); 
            }

            var existingTool = await _context.Tools.FindAsync(id);
            if (existingTool == null)
            {
                return NotFound();
            }

            // Uppdatera de egenskaper som är tillåtna att ändra
            existingTool.Name = updatedTool.Name;
            existingTool.Description = updatedTool.Description;
            existingTool.RentalPrice = updatedTool.RentalPrice;

            // Sätt UserId till den inloggade användaren
            existingTool.UserId = userId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToolExists(id))
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

        // DELETE: api/Tools/5
        [HttpDelete("{id}"), Authorize]
        public async Task<IActionResult> DeleteTool(int id)
        {
            var existingTool = await _context.Tools.FindAsync(id);
            if (existingTool == null)
            {
                return NotFound();
            }
            
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (existingTool.UserId != userId)
            {
                return Forbid(); 
            }

            _context.Tools.Remove(existingTool);
            await _context.SaveChangesAsync();

            return NoContent(); 
        }


        private bool ToolExists(int id)
        {
            return _context.Tools.Any(e => e.Id == id);
        }
    }
    
}