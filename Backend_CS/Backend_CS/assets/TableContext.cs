using System;
using System.Diagnostics;
using Backend_CS.Models;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Backend_CS.assets
{
	public class TableContext : DbContext
	{
		public TableContext(DbContextOptions<TableContext> options) : base(options)
		{
            Database.Migrate();
			
		}

        public DbSet<RequestData> requestDatas { get; set; }

		public DbSet<Worker> Workers { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<WorkGroup> WorkGroups { get; set; }
    }
}

