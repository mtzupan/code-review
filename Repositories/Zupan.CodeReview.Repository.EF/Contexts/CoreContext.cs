using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Zupan.CodeReview.Domains.Core;
using Zupan.CodeReview.Repository.EF.Utils;

namespace Zupan.CodeReview.Repository.EF.Contexts
{
    public class CoreContext : BaseContext
    {
        private string _connectionString;

        public CoreContext(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<DistributedServices> DistributedServices { get; set; }
        public DbSet<Edges> Edges { get; set; }
        public DbSet<Machines> Machines { get; set; }
        public DbSet<Profiles> Profiles { get; set; }
        public DbSet<UserProfiles> UserProfiles { get; set; }
        public DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionString, options =>
            {
                options.MigrationsHistoryTable("__CoreMigrationsHistory", "Core");
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