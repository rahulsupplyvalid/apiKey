using api_key_Authorize.Data;
using api_key_Authorize.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api_key_Authorize.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[AllowAnonymous]

    public class DataController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DataController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/data (protected by API key)
        [HttpGet]
        //[Authorize(Policy = "JwtOrApiKey")]
        //[Authorize(Policy = "JwtOnly")]
        //[Authorize(Policy = "ApiKeyOnly")]
        //[AllowAnonymous]    
        //[Authorize (Roles = "Admin")]
        //[Authorize(Policy = "ApiKeyOnly")]
        //[Authorize(Roles = "Admin")]
        [Authorize(Policy = "JwtOrApiKey")]
        //[Authorize(Policy = "ApiKeyOrJwt")]
        //[Authorize]
        public async Task<IActionResult> Get()
        {
            var data = await _context.ProtectedData.ToListAsync();
            return Ok(data);
        }

        // POST api/data (protected by API key)
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProtectedData data)
        {
            if (data == null)
            {
                return BadRequest("Invalid data.");
            }

            _context.ProtectedData.Add(data);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = data.Id }, data);
        }
    }
}
