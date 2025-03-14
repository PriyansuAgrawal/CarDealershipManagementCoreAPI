using CarDealershipManagement.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Interfaces.Repositories
{
    public interface ISalesmanRepository : IRepository<Salesman>
    {
        Task<IEnumerable<SalesRecord>> GetSalesRecordsAsync(int? month, int? year);
        Task<int> UpsertSalesRecordAsync(SalesRecord record);
        Task<bool> PopulateSampleDataAsync(int month, int year);
    }
}
