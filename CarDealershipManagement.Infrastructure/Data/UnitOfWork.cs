using CarDealershipManagement.Core.Interfaces.Repositories;
using CarDealershipManagement.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly SqlHelper _sqlHelper;
        private SqlConnection _connection;
        private SqlTransaction _transaction;
        private bool _disposed;

        private ICarModelRepository _carModels;
        private ISalesmanRepository _salesmen;
        private ICommissionRepository _commissions;
        private IUserRepository _users;

        public UnitOfWork(ISqlConnectionFactory connectionFactory, SqlHelper sqlHelper)
        {
            _connectionFactory = connectionFactory;
            _sqlHelper = sqlHelper;
        }

        public ICarModelRepository CarModels => _carModels ??= new CarModelRepository(_sqlHelper);

        public ISalesmanRepository Salesmen => _salesmen ??= new SalesmanRepository(_sqlHelper);

        public ICommissionRepository Commissions => _commissions ??= new CommissionRepository(_sqlHelper);

        public IUserRepository Users => _users ??= new UserRepository(_sqlHelper);

        public async Task BeginTransactionAsync()
        {
            _connection = _connectionFactory.CreateConnection();
            await _connection.OpenAsync();
            _transaction = _connection.BeginTransaction();
            _sqlHelper.SetTransaction(_transaction);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                _transaction?.Commit();
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }

                _sqlHelper.ClearTransaction();
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                _transaction?.Rollback();
            }
            finally
            {
                if (_transaction != null)
                {
                    _transaction.Dispose();
                    _transaction = null;
                }

                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                    _connection = null;
                }

                _sqlHelper.ClearTransaction();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _transaction?.Dispose();
                    _connection?.Dispose();
                }

                _transaction = null;
                _connection = null;
                _disposed = true;
            }
        }
    }
}
