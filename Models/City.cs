using masterapi.Base;
using masterapi.Helper;

public sealed class City : BaseEntity
{
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = RandomCode.GenerateCode("CT");
        public string Abbreviation { get; set; } = null!;
        public string DistrictCode { get; set; } 

}


