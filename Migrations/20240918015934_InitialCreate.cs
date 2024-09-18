using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using SingleTicketing.Helpers; // Import the namespace for PasswordHasher

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SingleTicketing.Migrations
{
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PasswordHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            // Use PasswordHasher to hash passwords
            var adminPassword1Hash = PasswordHasher.HashPassword("adminpass1");
            var adminPassword2Hash = PasswordHasher.HashPassword("adminpass2");
            var driverPassword1Hash = PasswordHasher.HashPassword("driverpass1");
            var driverPassword2Hash = PasswordHasher.HashPassword("driverpass2");
            var enforcerPassword1Hash = PasswordHasher.HashPassword("enforcerpass1");
            var enforcerPassword2Hash = PasswordHasher.HashPassword("enforcerpass2");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 1, adminPassword1Hash, "Admin", "admin1" },
                    { 2, adminPassword2Hash, "Admin", "admin2" },
                    { 3, driverPassword1Hash, "Driver", "driver1" },
                    { 4, driverPassword2Hash, "Driver", "driver2" },
                    { 5, enforcerPassword1Hash, "Enforcer", "enforcer1" },
                    { 6, enforcerPassword2Hash, "Enforcer", "enforcer2" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
