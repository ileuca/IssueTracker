using IssueTracker.Repository;
using System;

namespace IssueTracker.Models
{
    public class TeamGroupsModel
    {

        public int TeamGroupId { get; set; }
        public Guid TeamId { get; set; }
        public Guid UserId { get; set; }
        public Guid UserTeamRoleId { get; set; }
        public TeamGroupsModel()
        {
        }
    }
}