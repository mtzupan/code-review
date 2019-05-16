using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Domains;
using Zupan.CodeReview.Repository.EF.Contexts;
using Zupan.CodeReview.Repository.Interfaces;

namespace Zupan.CodeReview.Repository.EF.Repositories
{
    public class ModbusRepository<TEntity> : BaseRepository<TEntity>, IModbusRepository<TEntity> where TEntity : BaseEntity
    {
        private ModbusContext _modbusContext { get; set; }

        public ModbusRepository(ModbusContext context) : base(context)
        {
            _modbusContext = context;
        }


    }
}