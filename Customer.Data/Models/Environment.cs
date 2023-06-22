using Customer.Data.Common.Interfaces;
using System.Collections.Generic;

namespace Customer.Data.Models
{
    public class TenantEnvironment : ISoftDelete
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public bool IsBase { get; set; }
        public string? TenantId { get; set; }
       

        //Properties copied from Tenant
        public string? ServiceBusConnStr { get; set; }
        public string? PartitionKeyFormula { get; set; }
        public string? LogoPath { get; set; }
        public string? KeyVaultUri { get; set; }
        public string? FooterVersion { get; set; }
        public string? FooterTitle { get; set; }
        public string? CosmosDbConnStr { get; set; }
        public string? CommonCosmosConStr { get; set; }
        public string? PlApiUrl { get; set; }
        public string? PlanningStudioUrl { get; set; }
        public string? BlobStore { get; set; }
        public string? CommonStorageStr { get; set; }
        public string? ArchiveCosmosDbConnStr { get; set; }
        public bool CalculatePartitionKey { get; set; }
        public bool IsKeyVaultEnabled { get; set; }
        public bool IsInternalTenant { get; set; }
        public string? Url { get; set; }
        public bool IsActive { get; set; }
        public string? TenantTimeZone { get; set; }
        public string? PodTimeZone { get; set; }
        public bool EnableSLA { get; set; }
        public string? EnvironmentsDesignId { get; set; }
        public string? SyncFuncURL { get; set; }
        public bool AllowComparison { get; set; }
    }
}
