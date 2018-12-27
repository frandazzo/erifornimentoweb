using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using WebApplication3.Domain;
using WebApplication3.Domain.Security;

namespace WebApplication3.Db
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "1", Name = "ADMIN", NormalizedName = "ADMIN" });
            //builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = "2", Name = "USER", NormalizedName = "USER" });

            //builder.Entity<User>().HasData(new User {
            //    Id = "2",
            //    Name = "administrator",
            //    NormalizedUserName = "ADMINISTRATOR",
            //    Email="admin@erifornimento.it",
            //    IsActive = true,
            //    NormalizedEmail = "ADMIN@ERIFORNIMENTO.IT",
            //    UserName = "administrator",


            //});

        }

        public DbSet<Configurazione> Configurations { get; set; }


    }

}
