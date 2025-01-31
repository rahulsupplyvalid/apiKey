public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string OwnerEmail { get; set; } = null!;
    public string ProjectCode { get; set; } = null!;// Unique project code
    public string SecretKey { get; set; } = null!;// Unique secret key
}