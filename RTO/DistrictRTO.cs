using masterapi.Helper;

namespace masterapi.RTO
{
    public class DistrictRTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = RandomCode.GenerateCode("DT");
        public string Abbreviation { get; set; } = null!;
        public string? StateCode { get; set; }
    }
}
