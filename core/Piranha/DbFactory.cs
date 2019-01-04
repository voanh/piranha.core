#if DEBUG
/*
 * Copyright (c) 2017 Håkan Edling
 *
 * This software may be modified and distributed under the terms
 * of the MIT license.  See the LICENSE file for details.
 * 
 * http://github.com/piranhacms/piranha.core
 * 
 */

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics;

namespace Piranha
{
    /// <summary>
    /// Factory for creating a db context. Only used in dev mode
    /// when creating migrations.
    /// </summary>
    [NoCoverage]
    public class DbFactory : IDesignTimeDbContextFactory<Db>
    {
        /// <summary>
        /// Creates a new db context.
        /// </summary>
        /// <param name="args">The arguments</param>
        /// <returns>The db context</returns>
        public Db CreateDbContext(string[] args) 
        {
            //Debugger.Launch();
            var builder = new DbContextOptionsBuilder<Db>();
            builder.UseSqlServer("Server=103.98.148.238;Database=priahan;User ID=sa;Password=abcde12345-;");
            return new Db(builder.Options);
        }
    }
}
#endif