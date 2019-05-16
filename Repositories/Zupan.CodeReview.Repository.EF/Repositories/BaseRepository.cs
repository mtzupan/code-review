using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Zupan.CodeReview.Domains;
using Zupan.CodeReview.Dtos.Common;
using Zupan.CodeReview.Library.EfExtensions;
using Zupan.CodeReview.Repository.Interfaces;

namespace Zupan.CodeReview.Repository.EF.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;

        protected BaseRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        #region Get

        /// <summary>
        /// Gets an entity by its ID and site ID.
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="id">The Id of the Tentity</param>
        /// <returns>The TEntity</returns>
        public TEntity GetSingle(int id)
        {
            var query = GetSingle(withTracking: false, id: id);

            return query;
        }

        /// <summary>
        /// Gets an entity by a custom set of properties
        /// <para />
        /// EXAMPLE: GetBySingle(x => x.Property == Y)
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <returns>The entity that matches the set of properties</returns>
        public TEntity GetSingle(Expression<Func<TEntity, bool>> predicate)
        {
            var query = GetSingle(withTracking: false, predicate: predicate);

            return query;
        }

        /// <summary>
        /// Gets an entity by its  ID/SiteID/ClientID with Tracking enabled.
        /// <para />
        /// TO BE USED IF WE ARE UPDATING THIS ENTITY
        /// </summary>
        /// <param name="id">The Id of the Tentity</param>
        /// <returns>The TEntity</returns>
        public TEntity GetSingleWithTracking(int id)
        {
            var query = GetSingle(withTracking: true, id: id);

            return query;
        }

        /// <summary>
        /// Gets the entity included properties.
        /// </summary>
        /// <param name="includeProperties">The include properties.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public TEntity GetEntityIncludeProperties(TEntity entityToLoadParams, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            foreach (var property in includeProperties)
            {
                _context.Entry(entityToLoadParams).Reference(property).Load();
            }
            return entityToLoadParams;
        }

        /// <summary>
        /// Gets an entity by a custom set of properties
        /// <para />
        /// EXAMPLE: GetBySingle(x => x.Property == Y)
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <returns>The entity that matches the set of properties</returns>
        public TEntity GetSingleWithTracking(Expression<Func<TEntity, bool>> predicate)
        {
            var query = GetSingle(withTracking: true, predicate: predicate);

            return query;
        }


        /// <summary>
        /// Gets a single Entity.
        /// </summary>
        /// <param name="withTracking">if set to <c>true</c> [with tracking].</param>
        /// <param name="predicate">The predicate.</param>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        private TEntity GetSingle(bool withTracking = false, Expression<Func<TEntity, bool>> predicate = null, int? id = null)
        {
            var basePredicate = predicate ?? (x => x.Id == id);
            var dbSet = withTracking ? _dbSet : _dbSet.AsNoTracking();
            var query = dbSet.Where(basePredicate).FirstOrDefault();

            return query;
        }

        /// <summary>
        /// Gets a list of entities by a custom set of properties
        /// <para />
        /// EXAMPLE: Get(x => x.Property == Y)
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <returns>The list of entities that match the set of properties</returns>
        public virtual IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Get(predicate, withTracking: false);
        }

        /// <summary>
        /// Gets a list of entities by a custom set of properties with tracking
        /// <para />
        /// EXAMPLE: Get(x => x.Property == Y)
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <returns>The list of entities that match the set of properties</returns>
        public IQueryable<TEntity> GetWithTracking(Expression<Func<TEntity, bool>> predicate = null)
        {
            return Get(predicate, withTracking: true);
        }

        private IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate = null, bool withTracking = false)
        {
            var dbSet = withTracking ? _dbSet : _dbSet.AsNoTracking();
            var query = dbSet.AsNoTracking();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }

        /// <summary>
        /// Gets a pagedSet entity by a custom set of properties
        /// <para />
        /// EXAMPLE: GetBySingle(x => x.Property == Y, pagingProperties)
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <param name="properties">The paging properties</param>
        /// <returns>The entity that matches the set of properties</returns>
        public PagedSet<TEntity> GetPaged(Paging properties, Expression<Func<TEntity, bool>> predicate = null)
        {
            var result = _dbSet.AsNoTracking().SelectPage(properties, predicate);

            return result;
        }

        /// <summary>
        /// Gets a list of entities with specific inner entities included
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="includeProperties">The properties to include</param>
        /// <returns>A list of TEntities</returns>
        public IEnumerable<TEntity> GetOnlyIncludedProperties(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetIncluding(null, includeProperties);
        }

        /// <summary>
        /// Gets a list of entities with specific inner entities included
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="includeProperties">The properties to include</param>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <returns>A list of TEntities</returns>
        public IEnumerable<TEntity> GetOnlyIncludedProperties(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetIncluding(predicate, includeProperties);
        }

        /// <summary>
        /// Gets a pagedSet of entities with specific inner entities included
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="properties">The pagination properties</param>
        /// <param name="includeProperties">The properties to include</param>
        /// <returns>A list of TEntities</returns>
        public PagedSet<TEntity> GetOnlyIncludedPropertiesPaged(Paging properties,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetIncluding(null, includeProperties).SelectPage(properties);
        }

        /// <summary>
        /// Gets a pagedSet of entities with specific inner entities included
        /// <para />
        /// TO BE USED IF WE'RE NOT UPDATING THIS ENTITY
        /// </summary>
        /// <param name="properties">The pagination properties</param>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <param name="includeProperties">The properties to include</param>
        /// <returns>A list of TEntities</returns>
        public PagedSet<TEntity> GetOnlyIncludedPropertiesPaged(Paging properties,
            Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return GetIncluding(predicate, includeProperties).SelectPage(properties);
        }

        /// <summary>
        /// Private method that gets a TEntity with its included properties
        /// </summary>
        /// <param name="predicate">The predicate to search for the function</param>
        /// <param name="includeProperties">The properties to include</param>
        /// <returns>An IQueryable list of TEntities</returns>
        private IQueryable<TEntity> GetIncluding(Expression<Func<TEntity, bool>> predicate = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var queryable = _dbSet.AsNoTracking();

            queryable = includeProperties.Aggregate(queryable, (current, include) => current.Include(include));

            if (predicate != null)
            {
                queryable = queryable.Where(predicate);
            }

            return queryable;
        }
        #endregion

        #region Insert

        public bool InsertEntity(TEntity entity, string userLogin = null, bool saveChanges = true)
        {
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;
            entity.CreatedByUserLogin = userLogin;
            entity.ModifiedByUserLogin = userLogin;
            _context.Entry(entity).State = EntityState.Added;

            if (saveChanges)
            {
                return _context.SaveChanges() != 0;
            }
            return true;
        }

        public TEntity InsertAndReturnEntity(TEntity entity, string userLogin = null, bool saveChanges = true)
        {
            entity.CreatedOn = DateTime.Now;
            entity.ModifiedOn = DateTime.Now;
            entity.CreatedByUserLogin = userLogin;
            entity.ModifiedByUserLogin = userLogin;
            _context.Entry(entity).State = EntityState.Added;

            if(saveChanges)
            {
                _context.SaveChanges();
            }

            return entity;
        }

        public bool InsertBatchEntities(IEnumerable<TEntity> entities, string userLogin = null, bool saveChanges = true)
        {
            foreach (var entity in entities)
            {
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.CreatedByUserLogin = userLogin;
                entity.ModifiedByUserLogin = userLogin;
            }
            _context.AddRange(entities);

            if(saveChanges)
            {
                return _context.SaveChanges() != 0;
            }
            return true;
        }

        public IEnumerable<TEntity> InsertAndReturnBatchEntities(IEnumerable<TEntity> entities, string userLogin = null, bool saveChanges = true)
        {
            foreach (var entity in entities)
            {
                entity.CreatedOn = DateTime.Now;
                entity.ModifiedOn = DateTime.Now;
                entity.CreatedByUserLogin = userLogin;
                entity.ModifiedByUserLogin = userLogin;
            }
            _context.AddRange(entities);

            if (saveChanges)
            {
                _context.SaveChanges();
            }
            return entities;
        }
        #endregion

        #region Edit
        public TEntity UpdateAndReturnEntity(TEntity entity, string userLogin = null, bool saveChanges = true)
        {
            entity.ModifiedOn = DateTime.Now;
            entity.ModifiedByUserLogin = userLogin;
            _context.Entry(entity).State = EntityState.Modified;

            if (saveChanges)
            {
                _context.SaveChanges();
            }
            return entity;
        }

        public IEnumerable<TEntity> UpdateAndReturnBatchEntities(IEnumerable<TEntity> entities, string userLogin = null, bool saveChanges = true)
        {
            foreach (var entity in entities)
            {
                entity.ModifiedByUserLogin = userLogin;
                entity.ModifiedOn = DateTime.Now;
                _context.Entry(entity).State = EntityState.Modified;
            }

            if (saveChanges)
            {
                _context.SaveChanges();
            }
            return entities;
        }
        #endregion

        #region Delete
        public bool DeleteEntity(TEntity entity, bool saveChanges = true)
        {
            _context.Entry(entity).State = EntityState.Deleted;
            if(saveChanges)
            {
                return _context.SaveChanges() != 0;
            }
            return true;
        }
        #endregion

        #region Commit
        public bool Commit()
        {
            var success = _context.SaveChanges() != 0;
            return success;
        }
        #endregion
    }
}