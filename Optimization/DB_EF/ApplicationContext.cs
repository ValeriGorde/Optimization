using Microsoft.EntityFrameworkCore;
using Optimization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.DB_EF
{
    internal class ApplicationContext: DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<OptimizationMethod> OptimizationMethods { get; set; }
        public DbSet<Parameter> Parameters { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentParameter> AssignmentParameters { get; set; }
        public ApplicationContext() 
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseSqlite("Data Source = OptimizationDB.db");
        }
    }
}
