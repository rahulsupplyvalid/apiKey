using masterapi.Data;
using masterapi.DTO;
using masterapi.RTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace masterapi.Repository
{
    public sealed partial class CountryRepo
    {
        private readonly AuthMasterDbContext _context;

        public CountryRepo(AuthMasterDbContext context)
        {
            _context = context;
        }

        public async Task<CountryRTO[]?> GetAllCountryAsync()
        {
            var country = await _context.Countries.ToArrayAsync();
            var RTO = country.Select(c => new CountryRTO
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation
                
            }).ToArray();

            return RTO;
        }
        public async Task<CountryRTO[]?> GetCountryByIdAsync(string code)
        {
            var country = await _context.Countries.Where(x=>x.Code == code).ToArrayAsync();
            var RTO = country.Select(c => new CountryRTO
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation

            }).ToArray();

            return RTO;
        }
        public async Task<CountryDTO> AddcountryAsync([FromBody] CountryDTO countryDTO)
        {
            // Use the constructor that generates the Code automatically based on the Name
            var country = new Country
            {
               Name = countryDTO.Name,
                Abbreviation = countryDTO.Abbreviation, // Set Abbreviation from DTO
            };

            // Add the country object to the database
            await _context.Countries.AddAsync(country);
            await _context.SaveChangesAsync();
            return countryDTO;
        }

        public async Task<CountryDTO> UpdateCountry(string code, [FromBody] CountryDTO countryDTO)

        {
            // Find the country by its unique code
            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Code == code);

            if (country == null)
            {
                throw new KeyNotFoundException($"Country with Code '{code}' not found.");
            }

            // Update country properties
            country.Name = countryDTO.Name;
            country.Abbreviation = countryDTO.Abbreviation;

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the updated data
            return new CountryDTO
            {
                Name = country.Name,
                Abbreviation = country.Abbreviation
            };
        }


    }
}
