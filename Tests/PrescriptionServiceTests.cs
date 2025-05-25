using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Tutorial11.Data;
using Tutorial11.DTOs;
using Tutorial11.Models;
using Tutorial11.Services;
using Xunit;

namespace Tutorial11.Tests
{
    public class PrescriptionServiceTests
    {
        private AppDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppDbContext(options);
            context.Database.EnsureCreated();

            context.Doctors.Add(new Doctor { IdDoctor = 1, FirstName = "John", LastName = "Smith" });
            context.Medicaments.Add(new Medicament { IdMedicament = 1, Name = "TestMed", Description = "Desc", Type = "Type" });
            context.SaveChanges();

            return context;
        }

        [Fact]
        public async Task AddPrescription_ShouldAddPrescription_WhenValidRequest()
        {
            // Arrange
            var context = GetDbContext();
            var service = new PrescriptionService(context);

            var request = new PrescriptionRequestDto
            {
                IdDoctor = 1,
                IdPatient = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                Medicaments = new()
                {
                    new PrescriptionMedicamentDto { IdMedicament = 1, Dose = 2, Details = "Test" }
                }
            };

            // Act
            await service.AddPrescriptionAsync(request);

            // Assert
            var prescription = await context.Prescriptions.FirstOrDefaultAsync();
            Assert.NotNull(prescription);
            Assert.Equal(1, prescription.IdDoctor);
            Assert.Equal(1, prescription.IdPatient);
        }

        [Fact]
        public async Task AddPrescription_ShouldThrow_WhenMoreThan10Medicaments()
        {
            // Arrange
            var context = GetDbContext();
            var service = new PrescriptionService(context);

            var request = new PrescriptionRequestDto
            {
                IdDoctor = 1,
                IdPatient = 1,
                Date = DateTime.Now,
                DueDate = DateTime.Now.AddDays(1),
                Medicaments = new()
            };

            for (int i = 0; i < 11; i++)
            {
                request.Medicaments.Add(new PrescriptionMedicamentDto { IdMedicament = 1, Dose = 1, Details = "Test" });
            }

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.AddPrescriptionAsync(request));
        }
    }
}
