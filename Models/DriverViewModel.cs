namespace SingleTicketing.Models
{
    public class DriverViewModel
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? MiddleName { get; set; }

        public string? Cases { get; set; }
        public string? Status { get; set; }
        public string? LicenseNumber { get; set; }
        public DateTime? Birthdate { get; set; }
        public string? PlateNumber { get; set; }
        public string? LicenseRestrictions { get; set; }
        public string? TOP { get; set; }
        public string? TypeOfVehicle { get; set; }
        public int? Demerit { get; set; }
        public string? Email { get; set; }
        public string? Remarks { get; set; }
    }
}
