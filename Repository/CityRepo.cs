
using masterapi.Data;
using masterapi.DTO;
using masterapi.RTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace masterapi.Repository
{
    public sealed partial class CityRepo
    {

        private readonly AuthMasterDbContext _context;

        public CityRepo(AuthMasterDbContext context)
        {
            _context = context;
        }

        public async Task<CityRTO[]> GetAllCitiesAsync()
        {

            var cities = await _context.Cities
                 .Where(c => c.IsDeleted == false)  
                .ToArrayAsync();
            var cityRTO = cities.Select(c => new CityRTO
            
            {
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation,
                DistrictCode = c.DistrictCode
            }).ToArray();

            return cityRTO;
        }
        public async Task<CityRTO[]> GetCityByIdAsync(int id)
        {
            var cities = await _context.Cities
             .Where(c => c.IsDeleted == false)
             .ToArrayAsync();
            var cityRTO = cities.Select(c => new CityRTO
            {
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation,
                DistrictCode = c.DistrictCode
            }).ToArray();

            return cityRTO;
        }
        public async Task<CityDTO> AddCityAsync(CityDTO cityDTO)
        {
            var city = new City
            {
                Name = cityDTO.Name,
                Abbreviation = cityDTO.Abbreviation,
            };
            await _context.Cities.AddAsync(city);
            await _context.SaveChangesAsync();
            return cityDTO;
        }
        public async Task<string> UpdateCity(int id, CityDTO cityDTO)
        {
            var city = await _context.Cities.FindAsync(id);

            if (city == null)
            {
                return "City with the provided ID not found.";
            }

            if (cityDTO.Name != null) city.Name = cityDTO.Name;
            if (cityDTO.Abbreviation != null) city.Abbreviation = cityDTO.Abbreviation;


            await _context.SaveChangesAsync();
            return "City updated successfully.";
        }
        public async Task<bool> SoftDeleteCityAsync(int id)
        {
            var City = await _context.Cities.FindAsync(id);

            if (City == null)
            {
                return false;
            }

            City.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
    }
}