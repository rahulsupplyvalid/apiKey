using masterapi.Data;
using masterapi.DTO;
using masterapi.Repository;
using masterapi.RTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[Route("api/[controller]")]
[ApiController]
public class StateController : ControllerBase
{
    private readonly StateRepo stateRepo;

    public StateController(StateRepo stateRepo)
    {
        this.stateRepo = stateRepo;
    }



    // Add a new state with auto-generated code
    [HttpPost("PostState")]
    [Authorize (Roles = "Admin")]
    public async Task<ActionResult<State>> Post([FromForm] StateDto stateDto)
    {
        var postState = await stateRepo.CreateState(stateDto);
        if (postState == null)
        {
            return BadRequest("something went wrong");
        }
        return CreatedAtAction(nameof(GetStates), new { stateCode = postState.Code }, postState);
    }


    [HttpGet("GetStateByCode")]
    public async Task<ActionResult<State>> GetStateByCode(string stateCode)
    {
        var state = await stateRepo.GetStateByStateCode(stateCode);
        if(state == null)
        {
            return BadRequest("Something went Wrong");
        }
        return Ok(state);
    }



    // Update an existing state using Form Data and state code
    [HttpPut("UpdateState{stateCode}")]
    public async Task<ActionResult<State>> Put(string stateCode,[FromForm] StateDto stateDto)
    {
        var updatedState = await stateRepo.Put(stateCode,stateDto);
        if (updatedState == null)
        {
            return NotFound("State Not Found");
        }

        return Ok(updatedState);    
    }

    // Delete a state by code
    [HttpDelete("DeleteState")]
    public async Task<IActionResult> Delete(string stateCode)
    {
       var softDeletedState = await stateRepo.Delete(stateCode);
        if(softDeletedState == null)
        {
            return BadRequest("Not deleted");
        }
        return Ok(softDeletedState);
    }

    [HttpGet("GetStates")]
    public async Task<IActionResult> GetStates(string countryCode)
    {
       
        var states = await stateRepo.GetStates(countryCode);
        if (states == null || !states.Any())
        {
            return NotFound("No states found for the given country code.");
        }


        var stateRtos = states.Select(state => new StateRTO
        {
            Id = state.Id,
            Name = state.Name,
            Code = state.Code,
            Abbreviation = state.Abbreviation,
            CountryCode = state.CountryCode
        }).ToList();

        return Ok(stateRtos);
    }   
}
