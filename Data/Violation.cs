namespace SingleTicketing.Data
{
    public class Violation
    {

        public int Id { get; set; }

        public string? Violation_Name { get; set; }

        public string? Code { get; set; }

        public int? Fine_Amount { get; set; }
    }
}
