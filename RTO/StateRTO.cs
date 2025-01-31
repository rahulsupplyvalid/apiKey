namespace masterapi.RTO
{
    public class StateRTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; }  // Auto-generated code
        public string Abbreviation { get; set; }  // Short abbreviation of the state
        public string CountryCode { get; set; }  // Reference to the country code
        //public bool IsActive { get; set; }
    }
}
