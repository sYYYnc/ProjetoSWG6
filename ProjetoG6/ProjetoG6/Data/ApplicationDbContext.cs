using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjetoG6.Models;

namespace ProjetoG6.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<ProjetoG6.Models.Candidatos> Candidatos { get; set; }

        public DbSet<ProjetoG6.Models.Candidatura> Candidatura { get; set; }

        public DbSet<ProjetoG6.Models.ProgramaMobilidade> ProgramaMobilidade { get; set; }

        public DbSet<ProjetoG6.Models.Paises> Paises { get; set; }

        public DbSet<ProjetoG6.Models.ProgramaMobilidadePais> ProgramaMobilidadePais { get; set; }

        public DbSet<ProjetoG6.Models.AccountViewModels.Help> Help { get; set; }
       
    }
}
