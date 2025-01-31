using masterapi.Data;
using masterapi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private static List<Project> _projects = new List<Project>();
    private readonly AuthMasterDbContext DbContext;

    public ProjectController(AuthMasterDbContext DbContext)
    {
        this.DbContext = DbContext;
    }

    // Register a new project
    [HttpPost]
    [Route("Register Project")]
    public IActionResult RegisterProject([FromForm] ProjectDto projectDto)
    {
        if (string.IsNullOrEmpty(projectDto.Name) || string.IsNullOrEmpty(projectDto.OwnerEmail))
        {
            return BadRequest("Name and OwnerEmail are required.");
        }

        // Generate a unique project code and secret key
        string projectCode = GenerateProjectCode(projectDto.Name);
        string secretKey = GenerateSecretKey();

        var project = new Project
        {
            Name = projectDto.Name,
            Description = projectDto.Description,
            OwnerEmail = projectDto.OwnerEmail,
            ProjectCode = projectCode,
            SecretKey = secretKey
        };

        //_projects.Add(project);
        DbContext.Projects.Add(project);
        DbContext.SaveChanges();


        return CreatedAtAction(nameof(GetProjectByCode), new { projectCode = project.ProjectCode }, project);
    }

    // Get project by project code
    [HttpGet("{projectCode}")]
    [Authorize (Roles = "Admin")]
    public IActionResult GetProjectByCode(string projectCode)
    {
        var project = DbContext.Projects.FirstOrDefault(p => p.ProjectCode == projectCode);

        if (project == null)
        {
            return NotFound();
        }

        return Ok(project);
    }

    // Get all projects
    [HttpGet("GetAllProjects")]
    [Authorize(Roles = "Admin")]
    //[Authorize(Policy = "ApiKeyOnly")]


    public ActionResult<IEnumerable<Project>> GetAllProjects()
    {
        var projects = DbContext.Projects.ToList();
        return Ok(projects);
    }

    // Generate a unique project code
    private string GenerateProjectCode(string name)
    {
        // Use the first 3 characters of the project name and current timestamp for uniqueness
        string namePart = new string(name.Where(char.IsLetterOrDigit).Take(3).ToArray()).ToUpper();
        string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");

        return $"{namePart}{timestamp}";
    }

    // Generate a unique secret key
    private string GenerateSecretKey()
    {
        using (var hmac = new HMACSHA256())
        {
            // Generate 32 random bytes
            var randomBytes = new byte[32];
            RandomNumberGenerator.Fill(randomBytes);

            // Create a timestamp for uniqueness
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");

            // Combine the timestamp and random bytes, then hash the result
            string key = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(timestamp + Convert.ToBase64String(randomBytes))));

            return key;
        }
    }
}