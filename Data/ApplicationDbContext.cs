using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProperTax.Models;

namespace ProperTax.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }        
        public DbSet<Nieruchomosc> Nieruchomosci { get; set; }
        public DbSet<StawkaPodatku> StawkiPodatkow { get; set; }
        public DbSet<SumaPowierzchni> SumyPowierzchni { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Ustawia daty na dd-mm-rrrr bez godzin
            // Konfiguracja kolumn jako DATE
            modelBuilder.Entity<Nieruchomosc>()
                .Property(n => n.DataKupienia)
                .HasColumnType("date");

            modelBuilder.Entity<Nieruchomosc>()
                .Property(n => n.DataSprzedania)
                .HasColumnType("date");

            // Ustawienie roku jako klucza głównego
            modelBuilder.Entity<StawkaPodatku>()
                .HasKey(s => s.Rok);
            
            // Konfiguracja kolumny jako DATE
            modelBuilder.Entity<SumaPowierzchni>()
                .Property(n => n.RokMiesiac)
                .HasColumnType("date");

            // Ustawienie RokMiesiac jako klucza głównego
            modelBuilder.Entity<SumaPowierzchni>()
                .HasKey(s => s.RokMiesiac);
            

            // Wywołanie metody bazowej (zalecane)
            base.OnModelCreating(modelBuilder);        
        }        
    }
}
