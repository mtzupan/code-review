using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Zupan.CodeReview.Dtos.Common;

namespace Zupan.CodeReview.Repository.Interfaces
{
    public interface IBaseRepository<TEntity>
    {
        TEntity GetSingle(int id);

        TEntity GetSingle(Expression<Func<TEntity, bool>> predicate);

        TEntity GetSingleWithTracking(int id);

        TEntity GetEntityIncludeProperties(TEntity entityToLoadParams, params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetSingleWithTracking(Expression<Func<TEntity, bool>> predicate);

        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null);

        IQueryable<TEntity> GetWithTracking(Expression<Func<TEntity, bool>> predicate = null);

        PagedSet<TEntity> GetPaged(Paging properties, Expression<Func<TEntity, bool>> predicate = null);

        IEnumerable<TEntity> GetOnlyIncludedProperties(
            params Expression<Func<TEntity, object>>[] includeProperties);

        IEnumerable<TEntity> GetOnlyIncludedProperties(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        PagedSet<TEntity> GetOnlyIncludedPropertiesPaged(Paging properties,
           params Expression<Func<TEntity, object>>[] includeProperties);

        PagedSet<TEntity> GetOnlyIncludedPropertiesPaged(Paging properties,
            Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);

        bool InsertEntity(TEntity entity, string userLogin = null, bool saveChanges = true);

        TEntity InsertAndReturnEntity(TEntity entity, string userLogin = null, bool saveChanges = true);

        bool InsertBatchEntities(IEnumerable<TEntity> entities, string userLogin = null, bool saveChanges = true);

        IEnumerable<TEntity> InsertAndReturnBatchEntities(IEnumerable<TEntity> entities, string userLogin = null, bool saveChanges = true);

        TEntity UpdateAndReturnEntity(TEntity entity, string userLogin = null, bool saveChanges = true);

        IEnumerable<TEntity> UpdateAndReturnBatchEntities(IEnumerable<TEntity> entities, string userLogin = null, bool saveChanges = true);

        bool DeleteEntity(TEntity entity, bool saveChanges = true);

        bool Commit();
    }
}
