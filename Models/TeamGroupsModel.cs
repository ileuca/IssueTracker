using System;

namespace IssueTracker.Models
{
    public class TeamGroupsModel
    {
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserTeamRoleId { get; set; }
    }
}