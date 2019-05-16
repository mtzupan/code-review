using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Repository.Interfaces
{
    public interface IModbusRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
    {

    }
}