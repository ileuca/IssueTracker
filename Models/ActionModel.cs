using IssueTracker.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class ActionModel
    {
        public Guid ActionId { get; set; }
        public Guid IssueId { get; set; }
        [Required]
        public string ActionName { get; set; }
        [Required]
        public string ActionDescription { get; set; }
        [DateCheck("StartDate", "EndDate", Compare.LessThan)]
        public DateTime? StartDate { get; set; }
        [DateCheck("EndDate", "StartDate", Compare.GreaterThan)]
        public DateTime? EndDate { get; set; }
        public Guid StatusId { get; set; }

    }
}