using masterapi.Base;

public class District : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } 
    public string Code { get; set; }  // Auto-generated code
    public string Abbreviation { get; set; }  // Abbreviation of the district
    public string StateCode { get; set; }  // Reference to the state code
}
