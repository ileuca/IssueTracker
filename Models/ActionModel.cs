using System;

namespace IssueTracker.Models
{
    public class ActionModel
    {
        public Guid ActionId { get; set; }
        public Guid IssueId { get; set; }
        public string ActionName { get; set; }
        public string ActionDescription { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid StatusId { get; set; }

    }
}