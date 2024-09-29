using MultiTenant.Models;
using MultiTenant.Services;

public class TenantResolver
{
    private readonly RequestDelegate _next;

    public TenantResolver(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, ICurrentTenantService tenantService)
    {
        if (context.Request.Headers.TryGetValue("tenant", out var tenantId))
        {
            tenantService.TenantId = tenantId;
        }
        else
        {
            tenantService.TenantId = "DefaultTenant";
        }

        await _next(context);
    }
}
