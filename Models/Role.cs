public class Role
{
    public int Id { get; set; }
    public string RoleName { get; set; }
    public List<string> Permissions { get; set; }  // List of permissions assigned to the role
}
