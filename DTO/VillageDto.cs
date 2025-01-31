using System.ComponentModel;

namespace masterapi.DTO
{
    public class VillageDto
    {
        [DefaultValue("")]

        public string Name { get; set; } = null!;
        [DefaultValue("")]

        public string Code { get; set; } = null!;
        [DefaultValue("")]

        public string Abbreviation { get; set; } = null!;
        [DefaultValue("")]

        public string CityCode { get; set; } = null!;
    }
}
