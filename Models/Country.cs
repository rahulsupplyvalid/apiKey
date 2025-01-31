using masterapi.Base;
using masterapi.Helper;

public class Country : BaseEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string Code { get; set; } = RandomCode.GenerateCode("coun");
    public string? Abbreviation { get; set; }

}
