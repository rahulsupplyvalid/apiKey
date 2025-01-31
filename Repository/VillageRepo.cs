
using masterapi.Data;
using masterapi.DTO;
using masterapi.RTO;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver.Linq;

namespace masterapi.Repository
{
    public sealed partial class VillageRepo
    {

        private readonly AuthMasterDbContext _context;

        public VillageRepo(AuthMasterDbContext context)
        {
            _context = context;
        }

        public async Task<VillageRTO[]> GetAllVillageAsync()
        {

            var vill = await _context.Villages
                 .Where(c => c.IsDeleted == false)
                .ToArrayAsync();
            var VillageRTO = vill.Select(c => new VillageRTO
            {
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation,
                CityCode = c.CityCode
            }).ToArray();

            return VillageRTO;
        }
        public async Task<VillageDto> AddVillageAsync(VillageDto villageDto)
        {
            var vill = new Village
            {
                Name = villageDto.Name,
                Code = villageDto.Code,
                Abbreviation= villageDto.Abbreviation,
                CityCode= villageDto.CityCode
                
            };
            await _context.Villages.AddAsync(vill);
            await _context.SaveChangesAsync();
            return villageDto;
        }
        public async Task<bool> SoftDeleteVillageAsync(int id)
        {
            var vill = await _context.Villages.FindAsync(id);


            if (vill == null)
            {
                return false;
            }

            vill.IsDeleted = true;
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<string> UpdateVillage(int id, VillageDto villageDto)
        {
            var vill = await _context.Villages.FindAsync(id);

            if (vill == null)
            {
                return "Village with the provided ID not found.";
            }
            if (villageDto.Name != null) villageDto.Name = villageDto.Name;
            if (villageDto.Code != null) villageDto.Code = villageDto.Code;
            if (villageDto.Abbreviation != null) villageDto.Abbreviation = villageDto.Abbreviation;
            if (villageDto.CityCode != null) villageDto.CityCode = villageDto.CityCode;

            await _context.SaveChangesAsync();

            return "Village updated successfully.";
        }
        public async Task<VillageRTO[]> GetVillageByIdAsync(int id)
        {
            var vill = await _context.Villages
                .Where(c => c.IsDeleted == false && c.Id == id) 
                .ToArrayAsync();

            var VillageRTO = vill.Select(c => new VillageRTO
            {
                Name = c.Name,
                Code = c.Code,
                Abbreviation = c.Abbreviation,
                CityCode = c.CityCode,
            }).ToArray();

            return VillageRTO;
        }

    }
}
