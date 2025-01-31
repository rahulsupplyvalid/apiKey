using masterapi.DTO;
using masterapi.Repository;
using masterapi.RTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class DistrictController : ControllerBase
{
    public readonly DistrictRepo districtRepo;

    public DistrictController(DistrictRepo districtRepo)
    {
        this.districtRepo = districtRepo;
    }


    [HttpGet]
    public async Task<ActionResult<DistrictRTO[]?>> GetAllDistrict()
    {
        var district = await districtRepo.GetAllDistrictAsync();

        if (district == null || district.Length == 0)
        {
            return NotFound("No districts found.");
        }
        return Ok(district);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DistrictRTO[]?>> GetDistrictById(int id)
    {
        var district = await districtRepo.GetDistrictByIdAsync(id);
        if (district == null)
        {
            return NotFound("District not found.");
        }
        return Ok(district);
    }

    [HttpPost]
    public async Task<IActionResult> Adddistrict([FromForm] DistrictDto  districtDto)
    {
        if (districtDto == null)
        {
            return BadRequest("District data is null.");
        }
        var addedDis = await districtRepo.AddDistrictAsync(districtDto);

        return Ok(new { message = "District added successfully", District = addedDis });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> District(int id, [FromForm] DistrictDto districtDto)
    {
        if (districtDto == null)
        {
            return BadRequest(new { message = "District data cannot be null." });
        }
        var updated = await districtRepo.UpdateDistrict(id, districtDto);
        return Ok(new
        {
            District = updated
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDistrict(int id)
    {
        if (id == 0)
        {
            return BadRequest(new { message = "Unable to delete. Invalid Id provided." });
        }

        await districtRepo.SoftDeleteDistrictAsync(id);

        return Ok(new { message = "Successfully marked the District as deleted." });
    }

}


