using System;
using masterapi.Data;
using masterapi.Middleware;
using Microsoft.EntityFrameworkCore;

namespace api_key_Authorize.Repository
{
    public class DbRepository : IDbRepository
    {
        private readonly AuthMasterDbContext appDb;

        public DbRepository(AuthMasterDbContext appDb)
        {
            this.appDb = appDb;
        }

        // Method to validate API key
        public async Task<bool> ValidateApiKey(HttpContext httpContext)
        {
            // Skip validation for Swagger UI or related endpoints
            if (httpContext.Request.Path.StartsWithSegments("/swagger") ||
                httpContext.Request.Path.StartsWithSegments("/swagger/ui") ||
                httpContext.Request.Path.StartsWithSegments("/swagger/v1")
                //httpContext.Request.Path.StartsWithSegments("/api/Data")
                )
            {

                return true; // Skip API key validation for Swagger UI requests
            }

            // Retrieve all keys from the database
            var keys = await appDb.Projects
                                    .Select(p => p.SecretKey)
                                    .ToListAsync();

            if (keys == null || keys.Count == 0)
            {
                httpContext.Response.StatusCode = 403; // Forbidden
                await httpContext.Response.WriteAsync("Forbidden. No keys available in the database.");
                return false; // No keys found
            }

            // Combine all keys into a single string and then split into individual keys
            string combinedKeys = string.Join(",", keys);
            List<string> splitKeys = combinedKeys.Split(',')
                                                .Select(k => k.Trim()) // Clean up spaces
                                                .ToList();

            // Check if the "X-API-KEY" header exists and matches any of the keys in splitKeys
            if (!httpContext.Request.Headers.ContainsKey("X-API-KEY") ||
                !splitKeys.Contains(httpContext.Request.Headers["X-API-KEY"]))
            {
                httpContext.Response.StatusCode = 403; // Forbidden
                await httpContext.Response.WriteAsync("Forbidden. Invalid or missing API Key.");
                return false; // Invalid or missing API key
            }

            return true; // API key is valid
        }
    }

}