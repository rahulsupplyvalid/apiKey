using masterapi.DTO;
using masterapi.Repository;
using masterapi.RTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

//SHEKHAR SINGH
[Route("api/[controller]")]
[ApiController]
public class VillageController : ControllerBase
{
   public readonly VillageRepo villageRepo;

    public VillageController(VillageRepo villageRepo)
    {
        this.villageRepo = villageRepo;
    }

    [HttpGet]
    public async Task<ActionResult<VillageRTO[]?>> GetAllCities()
    {
        var Vill = await villageRepo.GetAllVillageAsync();
        if (Vill == null)
        {
            return NotFound("Village not found.");
        }
        return Ok(Vill);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VillageRTO[]?>> GetVillageById(int id)
    {
        var Vill = await villageRepo.GetVillageByIdAsync(id);
        if (Vill == null || !Vill.Any()) 
        {
            return NotFound("Village not found.");
        }
        return Ok(Vill);
    }

    [HttpPost]
    public async Task<IActionResult> AddVillage([FromForm] VillageDto villageDto)
    {
        if (villageDto == null)
        {
            return BadRequest("Village data is null.");
        }
        var Vill = await villageRepo.AddVillageAsync(villageDto);

        return Ok(new { message = "Village added successfully", Vill = villageDto });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateVillage(int id, [FromForm] VillageDto villageDto)
    {
        if (villageDto == null)
        {
            return BadRequest(new { message = "Village data cannot be null." });
        }
        var Vill = await villageRepo.UpdateVillage(id, villageDto);
        return Ok(new
        {
            Vill = villageDto
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteVillage(int id)
    {
        var Vill = await villageRepo.GetVillageByIdAsync(id);
        if (Vill == null || !Vill.Any()) 
        {
            return NotFound("Village not found.");
        }

        await villageRepo.SoftDeleteVillageAsync(id);

        return Ok(new { message = "Successfully marked the Village as deleted." });
    }
}
