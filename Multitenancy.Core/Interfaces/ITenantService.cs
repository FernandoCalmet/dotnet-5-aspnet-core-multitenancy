using Multitenancy.Core.Settings;

namespace Multitenancy.Core.Interfaces
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();

        public string GetConnectionString();

        public Tenant GetTenant();
    }
}
