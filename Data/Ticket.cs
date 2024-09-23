using System;

namespace SingleTicketing.Data
{
    public class Ticket
    {

        public int Id { get; set; }

        public string? DriverName { get; set; }

        public string? Date_Of_Birth { get; set; }

        public string? LicenseNumber { get; set; }

        public string? Contact_No { get; set; }
        
        public string? OR_Number { get; set; }

        public string? Address_of_the_Vehicle_Driver { get; set; }

        public string? Vehicle_Category { get; set; }

        public string? Code { get; set; }

        public string? Type_of_Violation    { get; set; }

        public string? Apprehending_Officer { get; set; }

        public string? MV_File {  get; set; }

        public int? Fine_Amount { get; set; }

        public DateTime? Amount_Due { get; set; }

        public DateTime? Date_Issued { get; set;}

        public string? Status { get; set; }
    }
}
