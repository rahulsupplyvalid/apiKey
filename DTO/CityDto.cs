using System.ComponentModel;

namespace masterapi.DTO
{
    public class CityDTO
    {
        [DefaultValue("")]
        public string Name { get; set; } = null!;
        [DefaultValue("")]
        public string Abbreviation { get; set; } = null!;


    }
}
