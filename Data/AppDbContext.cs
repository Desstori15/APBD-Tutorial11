using Microsoft.EntityFrameworkCore;
using Tutorial11.Models;

namespace Tutorial11.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<PrescriptionMedicament> PrescriptionMedicaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Явное указание ключей для всех сущностей
            modelBuilder.Entity<Doctor>().HasKey(d => d.IdDoctor);
            modelBuilder.Entity<Patient>().HasKey(p => p.IdPatient);
            modelBuilder.Entity<Medicament>().HasKey(m => m.IdMedicament);
            modelBuilder.Entity<Prescription>().HasKey(p => p.IdPrescription);

            // Конфигурация Many-to-Many для PrescriptionMedicament (у тебя уже есть)
            modelBuilder.Entity<PrescriptionMedicament>()
                .HasKey(pm => new { pm.IdMedicament, pm.IdPrescription });

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Prescription)
                .WithMany(p => p.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdPrescription);

            modelBuilder.Entity<PrescriptionMedicament>()
                .HasOne(pm => pm.Medicament)
                .WithMany(m => m.PrescriptionMedicaments)
                .HasForeignKey(pm => pm.IdMedicament);
        }

    }
}