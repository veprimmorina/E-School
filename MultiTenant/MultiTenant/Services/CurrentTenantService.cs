using Microsoft.EntityFrameworkCore;
using MultiTenant.Models;

namespace MultiTenant.Services
{
    public class CurrentTenantService : ICurrentTenantService
    {
        private readonly ITenantContext _tenantContext;

        public CurrentTenantService(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public string TenantId
        {
            get => _tenantContext.TenantId;
            set => _tenantContext.TenantId = value; // Allow setting TenantId
        }
    }
}
