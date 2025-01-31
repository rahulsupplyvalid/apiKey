// [Route("api/[controller]")]
// [ApiController]
// public class SuperAdminController : ControllerBase
// {
//     private readonly ApplicationDbContext _context;

//     public SuperAdminController(ApplicationDbContext context)
//     {
//         _context = context;
//     }

//     // Create a new user and assign a role (SuperAdmin only)
//     [HttpPost("create-user")]
//     public ActionResult<User> CreateUser([FromForm] string username, [FromForm] string email, [FromForm] string password, [FromForm] string role)
//     {
//         var superAdmin = _context.Users.FirstOrDefault(u => u.Role == "SuperAdmin");
//         if (superAdmin == null)
//         {
//             return Unauthorized("Super Admin is not found.");
//         }

//         if (role != "Admin" && role != "User")  // Define acceptable roles
//         {
//             return BadRequest("Invalid role.");
//         }

//         var newUser = new User
//         {
//             Username = username,
//             Email = email,
//             PasswordHash = HashPassword(password),
//             Role = role
//         };

//         _context.Users.Add(newUser);
//         _context.SaveChanges();

//         return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
//     }

//     // Get all users
//     [HttpGet("users")]
//     public ActionResult<IEnumerable<User>> GetAllUsers()
//     {
//         return Ok(_context.Users);
//     }

//     // Get user by Id
//     [HttpGet("users/{id}")]
//     public ActionResult<User> GetUserById(int id)
//     {
//         var user = _context.Users.FirstOrDefault(u => u.Id == id);
//         if (user == null)
//         {
//             return NotFound();
//         }
//         return Ok(user);
//     }

//     // Assign a role to a user (Super Admin only)
//     [HttpPost("assign-role")]
//     public ActionResult<User> AssignRole([FromForm] int userId, [FromForm] string role)
//     {
//         var superAdmin = _context.Users.FirstOrDefault(u => u.Role == "SuperAdmin");
//         if (superAdmin == null)
//         {
//             return Unauthorized("Super Admin is not found.");
//         }

//         var user = _context.Users.FirstOrDefault(u => u.Id == userId);
//         if (user == null)
//         {
//             return NotFound("User not found.");
//         }

//         if (role != "Admin" && role != "User")
//         {
//             return BadRequest("Invalid role.");
//         }

//         user.Role = role;
//         _context.SaveChanges();

//         return Ok(user);
//     }

//     // Hash the password (for security)
//     private string HashPassword(string password)
//     {
//         using (var sha256 = SHA256.Create())
//         {
//             var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
//             return Convert.ToBase64String(hashedBytes);
//         }
//     }
// }
