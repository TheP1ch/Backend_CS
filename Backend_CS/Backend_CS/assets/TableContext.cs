using System;
using Backend_CS.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend_CS.assets
{
	public class TableContext : DbContext
	{
		public TableContext(DbContextOptions<TableContext> options) : base(options)
		{
			Database.EnsureCreated();
			
		}

		public DbSet<RequestData> requestDatas { get; set; }

		public DbSet<Worker> Workers { get; set; }

        public DbSet<Request> Requests { get; set; }

        public DbSet<WorkGroup> WorkGroups { get; set; }
    }
}

