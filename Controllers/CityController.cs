using masterapi.Data;
using masterapi.DTO;
using masterapi.Repository;

//using masterapi.Repository;
using masterapi.RTO;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

//SHEKHAR SINGH
[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{

    private readonly CityRepo cityRepo;

    public CityController(CityRepo cityRepo)
    {
        this.cityRepo = cityRepo;
    }

    [HttpGet]
    public async Task<ActionResult<CityRTO[]?>> GetAllCities()
    {
        var cities = await cityRepo.GetAllCitiesAsync();
        if (cities == null)
        {
            return NotFound("City not found.");
        }
        return Ok(cities);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CityRTO[]?>> GetCityById(int id)
    {
        var city = await cityRepo.GetCityByIdAsync(id);
        if (city == null)
        {
            return NotFound("City not found.");
        }
        return Ok(city);
    }

    [HttpPost]
    public async Task<IActionResult> AddCity([FromForm] CityDTO cityDTO)
    {
        if (cityDTO == null)
        {
            return BadRequest("City data is null.");
        }
        var addedCity = await cityRepo.AddCityAsync(cityDTO);

        return Ok(new { message = "City added successfully", city = addedCity });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCity(int id, [FromForm] CityDTO cityDTO)
    {
        if (cityDTO == null)
        {
            return BadRequest(new { message = "City data cannot be null." });
        }
        var updatedCity = await cityRepo.UpdateCity(id, cityDTO);
        return Ok(new
        {
            city = updatedCity
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        if (id == 0)
        {
            return BadRequest(new { message = "Unable to delete. Invalid Id provided." });
        }

        await cityRepo.SoftDeleteCityAsync(id);

        return Ok(new { message = "Successfully marked the city as deleted." });
    }

}

