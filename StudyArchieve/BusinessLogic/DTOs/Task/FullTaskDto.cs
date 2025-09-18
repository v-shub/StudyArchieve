using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.DTOs.Task
{
    public class FullTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ConditionText { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }

        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;

        public int? AcademicYearId { get; set; }
        public string? AcademicYearLabel { get; set; }

        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;

        public List<AuthorDto> Authors { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
        public List<SolutionDto> Solutions { get; set; } = new();
        public List<TaskFileDto> TaskFiles { get; set; } = new();
    }
}
