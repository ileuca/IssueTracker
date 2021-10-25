﻿using IssueTracker.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace IssueTracker.Models
{
    public class IssueModel
    {
        public Guid IssueId { get; set; }
        public Guid ProjectId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string IssueName { get; set; }
        [Required]
        public string IssueDescription { get; set; }

        [DateCheck("StartDate", "EndDate", Compare.LessThan)]
        public DateTime? StartDate { get; set; }

        [DateCheck("EndDate", "StartDate", Compare.GreaterThan)]
        public DateTime? EndDate { get; set; }
        public Guid StatusId { get; set; }
    }
}