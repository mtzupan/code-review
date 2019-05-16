﻿using System.IO;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.EntityFrameworkCore.Design;
using Zupan.CodeReview.Repository.EF.Contexts;

namespace Zupan.CodeReview.Repository.EF.Migrations
{
    public class MigrationsCoreContextFactory : IDesignTimeDbContextFactory<CoreContext>
    {

        public CoreContext CreateDbContext(string[] args)
        {
            return Create();
        }

        private CoreContext Create()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            var config = builder.Build();

            var connstr = config.GetConnectionString("MimsDB");

            if (string.IsNullOrWhiteSpace(connstr))
            {
                throw new InvalidOperationException(
                    "Could not find a connection string named MimsDB.");
            }
            return new CoreContext(connstr);
        }
    }
}

