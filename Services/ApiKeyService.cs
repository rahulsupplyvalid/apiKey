using System;
using System.Collections.Generic;
using System.Linq;

public class ApiKeyService
{
    private static List<ApiKey> _apiKeys = new List<ApiKey>();

    public string GenerateApiKey(string createdBy)
    {
        var key = Guid.NewGuid().ToString();
        _apiKeys.Add(new ApiKey { Key = key, CreatedAt = DateTime.UtcNow, CreatedBy = createdBy });
        return key;
    }

    public bool ValidateApiKey(string apiKey)
    {
        return _apiKeys.Any(key => key.Key == apiKey);
    }
}
