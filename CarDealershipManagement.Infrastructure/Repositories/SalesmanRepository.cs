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
    public class SalesmanRepository : ISalesmanRepository
    {
        private readonly SqlHelper _sqlHelper;

        public SalesmanRepository(SqlHelper sqlHelper)
        {
            _sqlHelper = sqlHelper;
        }

        public async Task<Salesman> GetByIdAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@SalesmanId", id)
            };

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetSalesmanById", parameters);

            if (await reader.ReadAsync())
            {
                return new Salesman
                {
                    SalesmanId = reader.GetInt32(reader.GetOrdinal("SalesmanId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    PreviousYearSales = reader.GetDecimal(reader.GetOrdinal("PreviousYearSales")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
            }

            return null;
        }

        public async Task<IEnumerable<Salesman>> GetAllAsync()
        {
            var salesmen = new List<Salesman>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetAllSalesmen");

            while (await reader.ReadAsync())
            {
                salesmen.Add(new Salesman
                {
                    SalesmanId = reader.GetInt32(reader.GetOrdinal("SalesmanId")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    PreviousYearSales = reader.GetDecimal(reader.GetOrdinal("PreviousYearSales")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                });
            }

            return salesmen;
        }

        public async Task<int> AddAsync(Salesman entity)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@Name", entity.Name),
                    new SqlParameter("@PreviousYearSales", entity.PreviousYearSales)
            };

            return await _sqlHelper.ExecuteScalarAsync<int>("sp_CreateSalesMen", parameters);
        }

        public async Task<bool> UpdateAsync(Salesman entity)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@SalesmanId", entity.SalesmanId),
                    new SqlParameter("@Name", entity.Name),
                    new SqlParameter("@PreviousYearSales", entity.PreviousYearSales)
            };

            await _sqlHelper.ExecuteNonQueryAsync("sp_UpdateSalesman", parameters);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@SalesmanId", id)
            };

            await _sqlHelper.ExecuteNonQueryAsync("usp_DeleteSalesman", parameters);
            return true;
        }

        public async Task<IEnumerable<SalesRecord>> GetSalesRecordsAsync(int? month, int? year)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@Month", SqlDbType.Int) { Value = month ?? (object)DBNull.Value },
                    new SqlParameter("@Year", SqlDbType.Int) { Value = year ?? (object)DBNull.Value }
            };

            var records = new List<SalesRecord>();

            using var reader = await _sqlHelper.ExecuteReaderAsync("sp_GetSalesRecords", parameters);

            while (await reader.ReadAsync())
            {
                records.Add(new SalesRecord
                {
                    RecordId = reader.GetInt32(reader.GetOrdinal("RecordId")),
                    SalesmanId = reader.GetInt32(reader.GetOrdinal("SalesmanId")),
                    BrandId = reader.GetInt32(reader.GetOrdinal("BrandId")),
                    ClassId = reader.GetInt32(reader.GetOrdinal("ClassId")),
                    NumberOfCarsSold = reader.GetInt32(reader.GetOrdinal("NumberOfCarsSold")),
                    SaleMonth = reader.GetInt32(reader.GetOrdinal("SaleMonth")),
                    SaleYear = reader.GetInt32(reader.GetOrdinal("SaleYear")),
                    SalesmanName = reader.GetString(reader.GetOrdinal("SalesmanName")),
                    BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                    ClassName = reader.GetString(reader.GetOrdinal("ClassName"))
                });
            }

            return records;
        }

        public async Task<int> UpsertSalesRecordAsync(SalesRecord record)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@SalesmanId", record.SalesmanId),
                    new SqlParameter("@BrandId", record.BrandId),
                    new SqlParameter("@ClassId", record.ClassId),
                    new SqlParameter("@NumberOfCarsSold", record.NumberOfCarsSold),
                    new SqlParameter("@Month", record.SaleMonth),
                    new SqlParameter("@Year", record.SaleYear)
            };

            return await _sqlHelper.ExecuteScalarAsync<int>("sp_UpsertSalesRecord", parameters);
        }

        public async Task<bool> PopulateSampleDataAsync(int month, int year)
        {
            var parameters = new SqlParameter[]
            {
                    new SqlParameter("@Month", month),
                    new SqlParameter("@Year", year)
            };

            await _sqlHelper.ExecuteNonQueryAsync("sp_PopulateSampleData", parameters);
            return true;
        }
    }
}
