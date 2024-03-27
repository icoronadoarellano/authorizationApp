using Authorization.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.DataAccess.Context
{
    public class PermissionContext : DbContext
    {
        public PermissionContext(DbContextOptions<PermissionContext> options) : base(options)
        {

        }

        public DbSet<PermissionType> PermissionTypes { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Permission>().HasKey(c => c.Id);

            modelBuilder.Entity<Permission>().HasOne(p => p.PermissionType)
              .WithMany(b => b.Permissions)
              .HasForeignKey(p => p.PermissionTypeId);

            modelBuilder.Entity<PermissionType>().HasKey(c => c.Id);

        }
    }
}
