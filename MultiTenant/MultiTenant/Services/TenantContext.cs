namespace MultiTenant.Services
{
    public class TenantContext : ITenantContext
    {
        public string TenantId { get; set; }
    }
}
