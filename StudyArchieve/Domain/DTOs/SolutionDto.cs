using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class SolutionDto
    {
        public int Id { get; set; }
        public string SolutionText { get; set; } = string.Empty;
        public DateTime DateAdded { get; set; }
        public User? UserAdded { get; set; }
        public List<SolutionFileDto> SolutionFiles { get; set; } = new();
    }
}
