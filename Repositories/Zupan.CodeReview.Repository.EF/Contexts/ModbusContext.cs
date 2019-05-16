using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Domains.Modbus;
using Zupan.CodeReview.Repository.EF.Utils;

namespace Zupan.CodeReview.Repository.EF.Contexts
{
    public class ModbusContext : BaseContext
    {
        private string _connectionString;

        public ModbusContext(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<ModbusContactConfigurations> ModbusContactConfigurations { get; set; }
        public DbSet<ModbusContacts> ModbusContacts { get; set; }
        public DbSet<ModbusDevices> ModbusDevices { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString, options =>
            {
                options.MigrationsHistoryTable("__ModbusMigrationsHistory", "Modbus");
            });
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.RemovePluralizingTableNameConvention();

            modelBuilder.AddCascadeDeleteConvention();

            modelBuilder.HasDefaultSchema("public");
        }
    }
}
