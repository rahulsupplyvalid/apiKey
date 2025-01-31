using masterapi.DTO;
using masterapi.Repositories.TokenRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{

    public LoginController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }

    private static List<User> _users = new List<User>();
    private static int _nextId = 1;
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    //private readonly ITokenRepository tokenRepository;

    // Register a new user (signup)
    [HttpPost("register")]
    [Authorize(Roles = "Admin")]

    public async Task<IActionResult> RegisterUser([FromForm] RegisterDto user)
    {
        if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.PasswordHash))
        {
            return BadRequest("Username, email, and password are required.");
        }
        var identityUser = new IdentityUser
        {
            UserName = user.Username,
            Email = user.Email
        };
        var identityResult = await userManager.CreateAsync(identityUser, user.PasswordHash);

        if (identityResult.Succeeded)
        {
            if (user.Roles != null && user.Roles.Any())
            {
                identityResult = await userManager.AddToRolesAsync(identityUser, user.Roles);

                if (identityResult.Succeeded)
                {
                    return CreatedAtAction(
                            nameof(Login),
                            new { username = identityUser.UserName },
                            new
                            {
                                Message = "User registered successfully",
                                User = identityUser,
                                Status = "Success",
                            });
                }
            }
        }

        return BadRequest("Something went wrong");

    }

    // Login and authenticate user
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string userEmail, [FromForm] string password)
    {
        var user = await userManager.FindByEmailAsync(userEmail);

        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, password);

            if (checkPasswordResult)
            {
                var roles = await userManager.GetRolesAsync(user);
                if (roles != null)
                {
                    var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());
                    return Ok(new
                    {
                        message = "Login Successfully",
                        Status = "Success",
                        Data = new
                        {
                            Token = jwtToken,
                            user.UserName,
                            EmailId = user.Email,
                            MobileNumber = user.PhoneNumber,
                            userRole = roles.ToList()
                        }

                    });

                }

            }
            else
            {
                return Ok(new
                {
                    message = "Password Incorrect",
                    Status = "failed",
                });
            }
        }

        return Ok(new
        {
            message = "Username does not exist",
            Status = "failed"
        });
    }

    // Get all users
    [HttpGet("users")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsersWithRoles()
    {
        try
        {
            // Fetch all users from the database
            var users = userManager.Users.ToList();

            if (users == null || !users.Any())
            {
                return Ok(new
                {
                    message = "No users found.",
                    Status = "Failed",
                    Data = new List<object>()
                });
            }

            // Build a list of users with their roles
            var userList = new List<object>();

            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user); // Fetch roles for each user
                userList.Add(new
                {
                    user.UserName,
                    user.Email,
                    user.PhoneNumber,
                    Role = roles.ToList(), // Include roles
                });
            }

            return Ok(new
            {
                message = "Users retrieved successfully.",
                Status = "Success",
                Data = userList
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new
            {
                message = "An error occurred while retrieving users.",
                Status = "Failed",
                Details = ex.Message
            });
        }
    }



    // Helper function to hash the password
    private string HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    // Helper function to verify the password
    private bool VerifyPassword(string password, string storedHash)
    {
        var hashedPassword = HashPassword(password);
        return storedHash.Equals(hashedPassword);
    }
}
