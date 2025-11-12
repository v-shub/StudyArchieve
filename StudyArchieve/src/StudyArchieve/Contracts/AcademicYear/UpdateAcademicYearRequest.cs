namespace StudyArchieveApi.Contracts.AcademicYear
{
    public class UpdateAcademicYearRequest
    {
        public int Id { get; set; }

        public string YearLabel { get; set; } = null!;
    }
}
