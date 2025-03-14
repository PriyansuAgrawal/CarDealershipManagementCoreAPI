using CarDealershipManagement.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Interfaces.Repositories
{
    public interface ICommissionRepository
    {
        Task<(IEnumerable<CommissionReport> Details, IEnumerable<CommissionSummary> Summary)>
            GenerateReportAsync(int? month, int? year);
    }
}
