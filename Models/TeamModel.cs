using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class TeamModel
    {
        public Guid TeamId { get; set; }
        [Required]
        public string TeamName { get; set; }
        [Required]
        public string TeamDescription { get; set; }
        public Guid CreatedBy { get; set; }

    }
}