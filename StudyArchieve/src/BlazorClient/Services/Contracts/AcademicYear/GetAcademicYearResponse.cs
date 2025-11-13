namespace BlazorClient.Services.Contracts.AcademicYear
{
    public class GetAcademicYearResponse
    {
        public int Id { get; set; }

        public string YearLabel { get; set; } = null!;
    }
}
