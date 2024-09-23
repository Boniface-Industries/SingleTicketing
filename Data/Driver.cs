namespace SingleTicketing.Data
{
    public class Driver
    {

        public int Id { get; set; }

        public required string UserName { get; set; }

        public required string Password { get; set; }

        public string? DriverName { get; set; }

        public string? LicenseNumber { get; set; }

        public string? Birthdate { get; set; }

        public int? PlateNumber { get; set; }

        public string? LicenseRestrictions { get; set; }

        public string? TOP {  get; set; }

        public string? TypeOfVehicle { get; set; }

        public int? Demerit {  get; set; }

        public string? Email { get; set; }
    }
}
