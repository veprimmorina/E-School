namespace MultiTenant.Services
{
    public interface ICurrentTenantService
    {
        string TenantId { get; set; }
    }
}
