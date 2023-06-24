using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Environment
{
    public class EnvironmentDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
        public bool IsBase { get; set; }
        public string? PlApiUrl { get; set; }
        public string? TenantId { get; set; }
        public string?   LogoPath { get; set; }
        public string? FooterVersion { get; set; }
        public string?   FooterTitle { get; set; }
        public bool CalculatePartitionKey { get; set; }
        public bool IsInternalTenant { get; set; }
        public string? Url { get; set; }
        public string? PlanningStudioUrl { get; set; }
        public bool IsActive { get; set; }
        public string? TenantTimeZone { get; set; }
        public string? PodTimeZone { get; set; }
        public bool EnableSLA { get; set; }
        public string? EnvironmentsDesignId { get; set; }
    }
}
