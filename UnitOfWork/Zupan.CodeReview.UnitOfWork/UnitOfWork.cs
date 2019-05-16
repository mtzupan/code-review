using System;
using System.Collections.Generic;
using System.Reflection;
using Zupan.CodeReview.Repository.EF.Contexts;
using Zupan.CodeReview.Repository.EF.Repositories;
using Zupan.CodeReview.Repository.Interfaces;

namespace Zupan.CodeReview.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CoreContext _coreContext;
        private readonly ModbusContext _modbusContext;
        private IDictionary<Tuple<string, string>, object> _coreRepositories;
        private IDictionary<Tuple<string, string>, object> _modbusRepositories;

        public UnitOfWork(ModbusContext modbusContext)
        {
            _modbusContext = modbusContext;
        }

        public UnitOfWork(CoreContext coreContext, ModbusContext modbusContext)
        {
            _coreContext = coreContext;
            _modbusContext = modbusContext;
        }

        /// <summary>
        /// Method to get the Core repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public ICoreRepository<TEntity> GetCoreRepository<TEntity>() where TEntity : class
        {
            var repo = GetCoreUnitOfWork<TEntity>(typeof(CoreRepository<>));

            if (repo != null)
            {
                return (ICoreRepository<TEntity>)repo;
            }

            var repositoryType = typeof(CoreRepository<>);

            var keyAdded = CreateNewCoreRepositoryInstanceOnHashtable<TEntity>(repositoryType);

            _coreRepositories.TryGetValue(keyAdded, out repo);
            return (ICoreRepository<TEntity>)repo;
        }

        /// <summary>
        /// Method to get the Modbus repository.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        public IModbusRepository<TEntity> GetModbusRepository<TEntity>() where TEntity : class
        {
            var repo = GetModbusUnitOfWork<TEntity>(typeof(ModbusRepository<>));

            if (repo != null)
            {
                return (IModbusRepository<TEntity>)repo;
            }

            var repositoryType = typeof(ModbusRepository<>);

            var keyAdded = CreateNewModbusRepositoryInstanceOnHashtable<TEntity>(repositoryType);

            _modbusRepositories.TryGetValue(keyAdded, out repo);
            return (IModbusRepository<TEntity>)repo;
        }

        /// <summary>
        /// Gets the core unit of work if the same is present on the repositories instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repoType">Type of the repo.</param>
        /// <returns></returns>
        private dynamic GetCoreUnitOfWork<TEntity>(MemberInfo repoType) where TEntity : class
        {
            if (_coreRepositories == null)
                _coreRepositories = new Dictionary<Tuple<string, string>, dynamic>();

            var entityTypeName = typeof(TEntity).Name;
            var repositoryTypeName = repoType.Name;
            var keyToSearch = Tuple.Create(entityTypeName, repositoryTypeName);

            return _coreRepositories.ContainsKey(keyToSearch) ? _coreRepositories[keyToSearch] : null;
        }

        /// <summary>
        /// Gets the modbus unit of work if the same is present on the repositories instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repoType">Type of the repo.</param>
        /// <returns></returns>
        private dynamic GetModbusUnitOfWork<TEntity>(MemberInfo repoType) where TEntity : class
        {
            if (_modbusRepositories == null)
                _modbusRepositories = new Dictionary<Tuple<string, string>, dynamic>();

            var entityTypeName = typeof(TEntity).Name;
            var repositoryTypeName = repoType.Name;
            var keyToSearch = Tuple.Create(entityTypeName, repositoryTypeName);

            return _modbusRepositories.ContainsKey(keyToSearch) ? _modbusRepositories[keyToSearch] : null;
        }

        /// <summary>
        /// Adds the repository to hash table if the repository is not have been initialized.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <param name="repoType">Type of the repo.</param>
        /// <param name="repositoryInstance">The repository instance.</param>
        /// <returns></returns>
        private Tuple<string, string> AddCoreRepositoryToHashtable(string entityTypeName, Type repoType, object repositoryInstance)
        {
            var repoTypeName = repoType.Name;
            var keyToAdd = Tuple.Create(entityTypeName, repoTypeName);
            _coreRepositories.Add(keyToAdd, repositoryInstance);
            return keyToAdd;
        }

        /// <summary>
        /// Adds the repository to hash table if the repository is not have been initialized.
        /// </summary>
        /// <param name="entityTypeName">Name of the entity type.</param>
        /// <param name="repoType">Type of the repo.</param>
        /// <param name="repositoryInstance">The repository instance.</param>
        /// <returns></returns>
        private Tuple<string, string> AddModbusRepositoryToHashtable(string entityTypeName, Type repoType, object repositoryInstance)
        {
            var repoTypeName = repoType.Name;
            var keyToAdd = Tuple.Create(entityTypeName, repoTypeName);
            _modbusRepositories.Add(keyToAdd, repositoryInstance);
            return keyToAdd;
        }

        /// <summary>
        /// Creates the new repository instance on hashtable.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repositoryType">Type of the repository.</param>
        /// <returns></returns>
        private Tuple<string, string> CreateNewCoreRepositoryInstanceOnHashtable<TEntity>(Type repositoryType) where TEntity : class
        {
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _coreContext);
            var keyAdded = AddCoreRepositoryToHashtable(typeof(TEntity).Name, repositoryType, repositoryInstance);
            return keyAdded;
        }

        /// <summary>
        /// Creates the new modbus repository instance on hashtable.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="repositoryType">Type of the repository.</param>
        /// <returns></returns>
        private Tuple<string, string> CreateNewModbusRepositoryInstanceOnHashtable<TEntity>(Type repositoryType) where TEntity : class
        {
            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _modbusContext);
            var keyAdded = AddModbusRepositoryToHashtable(typeof(TEntity).Name, repositoryType, repositoryInstance);
            return keyAdded;
        }

    }
}
