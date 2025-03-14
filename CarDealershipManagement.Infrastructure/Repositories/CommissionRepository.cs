using CarDealershipManagement.Core.Domain.Models;
using CarDealershipManagement.Core.Interfaces.Repositories;
using CarDealershipManagement.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Infrastructure.Repositories
{
    public class CommissionRepository : ICommissionRepository
    {
        private readonly SqlHelper _sqlHelper;

        public CommissionRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<(IEnumerable<CommissionReport> Details, IEnumerable<CommissionSummary> Summary)> GenerateReportAsync(int? month, int? year)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@Month", SqlDbType.Int) { Value = month ?? (object)DBNull.Value },
                    new SqlParameter("@Year", SqlDbType.Int) { Value = year ?? (object)DBNull.Value }
            };

            var details = new List<CommissionReport>();
            var summary = new List<CommissionSummary>();

            var dataSet = await _sqlHelper.ExecuteDataSetAsync("sp_GetCommissionReport", parameters);

            // First table is the details
            if (dataSet.Tables.Count > 0)
            {
                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    details.Add(new CommissionReport
                    {
                        SalesmanId = Convert.ToInt32(row["SalesmanId"]),
                        SalesmanName = row["SalesmanName"].ToString(),
                        BrandName = row["BrandName"].ToString(),
                        ClassName = row["ClassName"].ToString(),
                        CarsSold = Convert.ToInt32(row["CarsSold"]),
                        ModelPrice = Convert.ToDecimal(row["ModelPrice"]),
                        TotalSales = Convert.ToDecimal(row["TotalSales"]),
                        FixedCommission = Convert.ToDecimal(row["FixedCommission"]),
                        ClassCommission = Convert.ToDecimal(row["ClassCommission"]),
                        BonusCommission = Convert.ToDecimal(row["BonusCommission"]),
                        TotalCommission = Convert.ToDecimal(row["TotalCommission"])
                    });
                }
            }

            // Second table is the summary
            if (dataSet.Tables.Count > 1)
            {
                foreach (DataRow row in dataSet.Tables[1].Rows)
                {
                    summary.Add(new CommissionSummary
                    {
                        SalesmanId = Convert.ToInt32(row["SalesmanId"]),
                        SalesmanName = row["SalesmanName"].ToString(),
                        TotalCarsSold = Convert.ToInt32(row["TotalCarsSold"]),
                        TotalSalesAmount = Convert.ToDecimal(row["TotalSalesAmount"]),
                        TotalFixedCommission = Convert.ToDecimal(row["TotalFixedCommission"]),
                        TotalClassCommission = Convert.ToDecimal(row["TotalClassCommission"]),
                        TotalBonusCommission = Convert.ToDecimal(row["TotalBonusCommission"]),
                        GrandTotalCommission = Convert.ToDecimal(row["GrandTotalCommission"])
                    });
                }
            }

            return (details, summary);
        }
    }
}
