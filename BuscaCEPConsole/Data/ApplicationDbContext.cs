using System.Collections.Immutable;
using System.IO;
using BuscaCEPConsole.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BuscaCEPConsole.Data
{
    public class ApplicationDbContext : DbContext
    {
        public static IConfigurationRoot Configuration { get; set; }
        public DbSet<CepModel> Ceps { get; set; }

        public ApplicationDbContext()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();


            var result = Configuration["ConnectionStrings:DefaultConnection"];
        }

       


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CepModelConfigurations());
        }
    }
}