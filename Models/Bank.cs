using masterapi.Base;

namespace masterapi.Models
{
    public sealed class Bank :BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Ifsc { get; set; } = null!;
        public string Branch { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City1 { get; set; } = null!;
        public string City2 { get; set; } = null!;
        public string State { get; set; } = null!;
        public string? Stdcode { get; set; }
        public string? Phone { get; set; }
    }

}
