using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uno.Entities;

namespace Uno.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private Func<object, object> p;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public ApplicationDbContext(Func<object, object> p)
        {
            this.p = p;
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");
                entity.HasKey(e => e.AddressID);
                entity.HasOne(d => d.User).WithMany(p => p.Addresses);
               
            });

            builder.Entity<Log>(entity =>
            {
                entity.ToTable("Logs");
                entity.HasKey(e => e.LogID);
               // entity.Property(prop => prop.LogID).ValueGeneratedNever();
                });



            //builder.Entity<Address>()
            //    .ToTable("Address")
            //    .HasKey(e=>e.AddressID);



            //builder.Entity<Address>(entity =>
            //{
            //    entity.Property(e => e.Name)
            //        .IsRequired()
            //        .HasMaxLength(50);
            //});

            base.OnModelCreating(builder);
        }
    }
}
