using masterapi.Data;
using masterapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace masterapi.Seed
{
    public class DatabaseSeeder
    {

        private readonly AuthMasterDbContext context;
        private bool pending = false;

        public DatabaseSeeder(AuthMasterDbContext _context)
        {
            context = _context;
        }

        public async Task Run()
        {
            pending = false;
            await AddStates();
            await AddDistricts();
            await AddCities();
            await AddVillages();
            await AddBanks();
            await AddCountry();
            if (pending)
                await context.SaveChangesAsync();
        }

        private async Task AddStates()
        {
            var existing = await context.Set<State>()
                .Select(x => x.Code)
                .ToArrayAsync();

            var filtered = States.Where(x => !existing.Contains(x.Code))
                .ToArray();
            if (filtered.Length > 0)
            {
                await context.AddRangeAsync(filtered);
                pending = true;
            }
        }

        private async Task AddBanks()
        {
            var existing = await context.Set<Bank>()
                .Select(x => x.Id)
                .ToArrayAsync();

            var filtered = Banks.Where(x => !existing.Contains(x.Id))
                .ToArray();
            if (filtered.Length > 0)
            {
                await context.AddRangeAsync(filtered);
                pending = true;
            }
        }
        private async Task AddDistricts()
        {
            var existing = await context.Set<District>()
                .Select(x => x.Code)
                .ToArrayAsync();

            var filtered = Districts.Where(x => !existing.Contains(x.Code))
                .ToArray();

            if (filtered.Length > 0)
            {
                await context.AddRangeAsync(filtered);
                pending = true;
            }
        }
        private async Task AddCities()
        {
            var existing = await context.Set<City>()
                .Select(x => x.Code)
                .ToArrayAsync();

            var filtered = Cities.Where(x => !existing.Contains(x.Code))
                .ToArray();

            if (filtered.Length > 0)
            {
                await context.AddRangeAsync(filtered);
                pending = true;
            }
        }
        private async Task AddVillages()
        {
            var existing = await context.Set<Village>()
                .Select(x => x.Code)
                .ToArrayAsync();

            var filtered = Villages.Where(x => !existing.Contains(x.Code))
                .ToArray();

            if (filtered.Length > 0)
            {
                await context.AddRangeAsync(filtered);
                pending = true;
            }
        }
        private async Task AddCountry()
        {
            var existing = await context.Set<Country>()
                .Select(x => x.Code)
                .ToArrayAsync();

            var filtered = Countries.Where(x => !existing.Contains(x.Code))
                .ToArray();

            if (filtered.Length > 0)
            {
                await context.AddRangeAsync(filtered);
                pending = true;
            }
        }

        public static Village[] Villages
        {
            get
            {
                var jdoc = GetJsonString("Villages.json");
                var villages = jdoc.RootElement.GetProperty("Villages").Deserialize<Village[]>();
                return villages ?? Array.Empty<Village>();
            }
        }
        public static State[] States
        {
            get
            {
                var jdoc = GetJsonString("state.json");
                var states = jdoc.RootElement.GetProperty("State").Deserialize<State[]>();
                return states ?? Array.Empty<State>();
            }
        }
        public static District[] Districts
        {
            get
            {
                var jdoc = GetJsonString("district.json");
                var districts = jdoc.RootElement.GetProperty("District").Deserialize<District[]>();
                return districts ?? Array.Empty<District>();
            }
        }
        public static City[] Cities
        {
            get
            {
                var jdoc = GetJsonString("city.json");
                var cities = jdoc.RootElement.GetProperty("City").Deserialize<City[]>();
                return cities ?? Array.Empty<City>();
            }
        }

        public static Bank[] Banks
        {
            get
            {
                var jdoc = GetJsonString("bank.json");
                var banks = jdoc.RootElement.GetProperty("Bank").Deserialize<Bank[]>();
                return banks ?? [];
            }
        }

        public static Country[] Countries
        {
            get
            {
                var jdoc = GetJsonString("country.json");
                var country = jdoc.RootElement.GetProperty("Country").Deserialize<Country[]>();
                return country ?? [];
            }
        }
        private static JsonDocument GetJsonString(string filename)
        {
            string fullpath = Path.Combine(Directory.GetCurrentDirectory(), "Json", filename);
            using StreamReader reader = new(fullpath);
            string json = reader.ReadToEnd();
            var jdoc = JsonDocument.Parse(json);
            return jdoc;
        }
       
       

    }

}

