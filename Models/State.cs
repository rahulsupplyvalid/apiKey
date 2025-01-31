using Microsoft.Identity.Client;

public class State
{
    internal object StateName;

    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;// Auto-generated code
    public string Abbreviation { get; set; } = null!;  // Short abbreviation of the state
    public string CountryCode { get; set; } = null!;// Reference to the country code
  
}
