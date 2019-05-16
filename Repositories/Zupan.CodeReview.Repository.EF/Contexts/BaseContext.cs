using System;
using System.Collections.Generic;
using System.Text;

namespace Zupan.CodeReview.Repository.EF.Contexts
{
    using Microsoft.EntityFrameworkCore;

    public class BaseContext : DbContext
    {
        private readonly string _connectionString;

        public BaseContext(string connectionString)
        {
            this._connectionString = connectionString;
        }
    }
}
