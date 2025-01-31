using masterapi.Data;
using masterapi.DTO;
using masterapi.RTO;
using Microsoft.EntityFrameworkCore;

namespace masterapi.Repository
{
    public sealed class DistrictRepo
    {
        private readonly AuthMasterDbContext _context;

        public DistrictRepo(AuthMasterDbContext context)
        {
            _context = context;
        }

        public async Task<DistrictRTO[]> GetAllDistrictAsync()
        {
            var district = await _context.Districts
                .Where(c => c.IsDeleted == false)
                .ToArrayAsync();

            var RTO = district.Select(c => new DistrictRTO
            {
                Id = c.Id,
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation,
                StateCode = c.StateCode
            }).ToArray();
            return RTO;
        }

        public async Task<DistrictRTO[]> GetDistrictByIdAsync(int id)
        {
            var cities = await _context.Districts
             .Where(c => c.IsDeleted == false)
             .ToArrayAsync();
            var RTO = cities.Select(c => new DistrictRTO
            {
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation,
                StateCode = c.StateCode 
            }).ToArray();
            return RTO;
        }

        public async Task<DistrictRTO> AddDistrictAsync(DistrictDto districtDto)
        {
            var data = new District
            {
                Name = districtDto.Name,
                Abbreviation = districtDto.Abbreviation,
            };

            await _context.Districts.AddAsync(data);
            await _context.SaveChangesAsync();

            var result = new DistrictRTO
            {
                Id = data.Id,
                Name = data.Name,
                Code = data.Code, 
                Abbreviation = data.Abbreviation,
                StateCode = data.StateCode 
            };
            return result;
        }
        public async Task<string> UpdateDistrict(int id, DistrictDto districtDto)
        {
            var data = await _context.Districts.FindAsync(id);

            if (data == null)
            {
                return "District with the provided ID not found.";
            }

            if (districtDto.Name != null) data.Name = districtDto.Name;
            if (districtDto.Abbreviation != null) data.Abbreviation = districtDto.Abbreviation;


            await _context.SaveChangesAsync();
            return "District Updated successfully.";
        }
        public async Task<bool> SoftDeleteDistrictAsync(int id)
        {
            var data = await _context.Districts.FindAsync(id);

            if (data == null)
            {
                return false;
            }

            data.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }

    }
}
