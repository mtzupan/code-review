using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Domains;
using Zupan.CodeReview.Domains.Modbus;
using Zupan.CodeReview.Dtos.Modbus.Combinations;
using Zupan.CodeReview.Repository.EF.Contexts;
using Zupan.CodeReview.Repository.Interfaces;

namespace Zupan.CodeReview.Repository.EF.Repositories
{
    public class CoreRepository<TEntity> : BaseRepository<TEntity>, ICoreRepository<TEntity> where TEntity : BaseEntity
    {
        private CoreContext _coreContext { get; set; }

        public CoreRepository(CoreContext context) : base(context)
        {
            _coreContext = context;
        }
    }
}