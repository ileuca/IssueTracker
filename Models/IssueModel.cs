using System;

namespace IssueTracker.Models
{
    public class IssueModel
    {
        public Guid IssueId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string IssueName { get; set; }
        public string IssueDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid StatusId { get; set; }
    }
}