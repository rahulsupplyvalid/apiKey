namespace masterapi.Base
{
    public interface IBaseEntity
    {
        DateTime CreatedOn { get; set; }
        DateTime UpdatedOn { get; set; }
        Guid? CreatedBy { get; set; }
        Guid? UpdatedBy { get; set; }
        bool IsDeleted { get; set; }
        bool IsActive { get; set; }

    }
}
