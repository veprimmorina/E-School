namespace MultiTenant.Services
{
    public interface ITenantContext
    {
        string TenantId { get; set; }
    }
}
