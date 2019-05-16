using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Repository.Interfaces;

namespace Zupan.CodeReview.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICoreRepository<TEntity> GetCoreRepository<TEntity>() where TEntity : class;

        IModbusRepository<TEntity> GetModbusRepository<TEntity>() where TEntity : class;
    }
}
