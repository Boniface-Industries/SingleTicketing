using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace SingleTicketing.Data
{
    public static class DriverSeeder
    {
        public static void SeedDrivers(IServiceProvider serviceProvider)
        {
            using (var context = new MyDbContext(serviceProvider.GetRequiredService<DbContextOptions<MyDbContext>>()))
            {
                // Check if the database is created and if there are any existing drivers
                context.Database.EnsureCreated();

                if (context.Drivers.Any())
                {
                    // Database has been seeded
                    return;
                }

                var drivers = new[]
                {
                    new Driver
                    {
                        UserName = "driver1",
                        FirstName = "John  ",
                          LastName = "  Doe",
                            MiddleName = "Joe",
                        LicenseNumber = "AB123456",
                        Birthdate = "1980-01-01",
                        PlateNumber = 1234,
                        LicenseRestrictions = "None",
                        TOP = "Valid",
                        TypeOfVehicle = "Sedan",
                        Demerit = 0,
                        Email = "johndoe@example.com"
                    },
                    new Driver
                    {
                        UserName = "driver2",
                         FirstName = "John  ",
                       LastName = "  Smith",
                        MiddleName = "  Doe",
                        LicenseNumber = "CD654321",
                        Birthdate = "1985-05-15",
                        PlateNumber = 5678,
                        LicenseRestrictions = "None",
                        TOP = "Valid",
                        TypeOfVehicle = "SUV",
                        Demerit = 1,
                        Email = "janesmith@example.com"
                    },
                    // Add more drivers as needed
                };

                context.Drivers.AddRange(drivers);
                context.SaveChanges();
            }
        }
    }
}
