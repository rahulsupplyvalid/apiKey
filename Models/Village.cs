using masterapi.Base;

public sealed class Village : BaseEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!; 
    public string Abbreviation { get; set; } = null!; 
    public string CityCode { get; set; } = null!; 
}
