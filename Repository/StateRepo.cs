using masterapi.Data;
using masterapi.DTO;
using masterapi.RTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace masterapi.Repository
{
    public sealed partial class StateRepo
    {
        private readonly AuthMasterDbContext masterDbContext;

        public StateRepo(AuthMasterDbContext masterDbContext)
        {
            this.masterDbContext = masterDbContext;
        }



        public async Task<List<State>> GetStates([FromForm] string countryCode )
        {

            //var filteredStates = await masterDbContext.states.FirstOrDefaultAsync(a => a.CountryCode == countryCode);
            var filteredStates = await masterDbContext.States.Where(a => a.CountryCode == countryCode).ToListAsync();

            return filteredStates;
        }

        public async Task<State> CreateState(StateDto stateDto)
        {
            // Generate state code based on current date and a 2-letter reference
            string stateCode = GenerateStateCode(stateDto.name);

            // Create the state object and assign an auto-generated ID
            var state = new State
            {
                Name = stateDto.name,
                Code = stateCode,
                Abbreviation = stateDto.abbreviation,
                CountryCode = stateDto.countryCode
            };

            await masterDbContext.States.AddAsync(state);
            await masterDbContext.SaveChangesAsync();
            return state;
        }


        public async Task<State> Put(string stateCode,StateDto stateDto)
        {
            var state = masterDbContext.States.FirstOrDefault(s => s.Code == stateCode);

            if (state == null)
            {
                return null;
            }

            // Generate new code based on name (or you can keep it static)
            string stateCodeGenerated = GenerateStateCode(stateDto.name);

            state.Name = stateDto.name;
            state.Code = stateCodeGenerated;
            state.Abbreviation = stateDto.abbreviation;
            state.CountryCode = stateDto.countryCode;

            masterDbContext.States.Update(state);
            await masterDbContext.SaveChangesAsync();
            return state;
        }

        public async Task<State> Delete(string stateCode)
        {
            var state = masterDbContext.States.FirstOrDefault(s => s.Code == stateCode);

            if (state == null)
            {
                return null;
            }


            // state.IsActive = true;
            // state.DeletedAt = DateTime.UtcNow;
            masterDbContext.States.Update(state);
            await masterDbContext.SaveChangesAsync();



            //masterDbContext.states.Remove(state);

            //await masterDbContext.SaveChangesAsync();
            return state;

        }

        public async Task<List<State>> GetStateByStateCode(string stateCode)
        {
            //var state = masterDbContext.states.FirstOrDefault(s => s.Code == stateCode);
            var state = await masterDbContext.States.Where(s => s.Code == stateCode).ToListAsync();

            if (state == null)
            {
                return null;
            }

            return state;
        }



        // Generate state code as "2-letter reference + DDMMYYYY"

        private string GenerateStateCode(string stateName)
        {
            // Take the first 2 letters of the state name (in uppercase)
            string reference = stateName.Length >= 2 ? stateName.Substring(0, 2).ToUpper() : stateName.ToUpper();

            // Get current date in DDMMYYYY format
            string dateCode = DateTime.Now.ToString("ddMMyyyy");

            // Combine the reference and date to create the code
            string stateCode = $"{reference}{dateCode}";

            return stateCode;
        }
    }
}
