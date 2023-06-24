using Customer.Model.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Customer.Model.Environment
{
    public class EnvironmentCompanyDto
    {
        public List<CompanyDto> Companies { get; set; }
        public EnvironmentDto Environment { get; set; }
    }
}
