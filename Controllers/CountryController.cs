using masterapi.Data;
using masterapi.DTO;
using masterapi.Repository;
using masterapi.RTO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class CountryController : ControllerBase
{
    private readonly CountryRepo countryRepo;

    public CountryController(CountryRepo countryRepo)
    {
        this.countryRepo = countryRepo;
    }

    [HttpGet("countrylist")]
    public async Task<ActionResult<CountryRTO[]?>> GetAllCountries()
    {
        var con = await countryRepo.GetAllCountryAsync();
        if (con == null)
        {
            return NotFound("City not found.");
        }
        return Ok(con);
    }


    [HttpGet("{code}")]
    public async Task<ActionResult<CountryRTO[]?>> GetcountryById(string code)
    {
        var cont = await countryRepo.GetCountryByIdAsync(code);
        if (cont == null)
        {
            return NotFound("Country not found.");
        }
        return Ok(cont);
    }

    [HttpPost("addcountry")]
    public async Task<IActionResult> Addcounry([FromForm] CountryDTO countryDTO)
    {
        if (countryDTO == null)
        {
            return BadRequest("Country data is null.");
        }
        var addedcountry = await countryRepo.AddcountryAsync(countryDTO);
        return Ok(new { message = "Country added successfully", city = addedcountry });
    }

    [HttpPut("{code}/update")]
    public async Task<IActionResult> Updatecountryy(string code, [FromForm] CountryDTO countryDTO)
    {
        if (countryDTO == null)
        {
            return BadRequest("Country data is null.");
        }
        var existingCity = await countryRepo.UpdateCountry(code,countryDTO);
        return Ok(new { message = "Successfully updated the country." });
    }

}
