namespace masterapi.Base
{
    public abstract class BaseEntity
    {
        public DateTime CreatedOn { get; set; } = DateTime.Now.ToUniversalTime();
        public DateTime UpdatedOn { get; set; } = DateTime.Now.ToUniversalTime();
        public Guid? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; } = false;
        public bool IsActive { get; set; } = true;
    }
}
