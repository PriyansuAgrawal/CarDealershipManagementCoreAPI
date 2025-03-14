using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealershipManagement.Core.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICarModelRepository CarModels { get; }
        ISalesmanRepository Salesmen { get; }
        ICommissionRepository Commissions { get; }
        IUserRepository Users { get; }

        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
